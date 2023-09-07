// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Platform;
using Epic.OnlineServices.Samples.SDK;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Epic.OnlineServices.Samples
{
	public class PlatformApplication : IDisposable
	{
		private static Lazy<PlatformApplication> s_PlatformApplication = new Lazy<PlatformApplication>();
		public static PlatformApplication Instance
		{
			get { return s_PlatformApplication.Value; }
		}

		public PlatformInterface PlatformInterface { get; private set; }

		public DateTimeOffset LastTickTime { get; private set; } = DateTimeOffset.UtcNow;

		private IHost m_Host;

		private CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();

		private List<SDKRequest> s_CurrentRequests = new List<SDKRequest>();

		private List<VoiceRoom> s_VoiceRooms = new List<VoiceRoom>();

		private object s_Lock = new object();

		private bool IsDisposed;

		private const float c_HeartbeatExpireSeconds = 60;

		public PlatformApplication()
		{
		}

		~PlatformApplication()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			OnDisposing();
			IsDisposed = true;
		}

		private void OnDisposing()
		{
			if (PlatformInterface != null)
			{
				PlatformInterface.Release();
				PlatformInterface = null;
			}

			PlatformInterface.Shutdown();

			if (m_Host != null)
			{
				m_Host.Dispose();
			}
		}

		public async void Run()
		{
			m_Host = CreateHostBuilder().Build();
			await m_Host.StartAsync(m_CancellationTokenSource.Token);

			bool isServer = true;
			PlatformInterface = EpicApplication.Initialize(PlatformFlags.DisableOverlay, isServer);

			while (!m_CancellationTokenSource.IsCancellationRequested)
			{
				// Fire off any requests that haven't yet started
				lock (s_Lock)
				{
					foreach (var currentRequest in s_CurrentRequests)
					{
						if (!currentRequest.IsStarted)
						{
							currentRequest.Start();
						}
					}
				}

				// Remove expired rooms
				lock (s_Lock)
				{
					foreach (var voiceRoom in s_VoiceRooms.ToArray())
					{
						if (voiceRoom.LastHeartbeatTime.AddSeconds(c_HeartbeatExpireSeconds) < DateTimeOffset.UtcNow)
						{
							s_VoiceRooms.Remove(voiceRoom);
							Log.WriteLine($"Voice room '{voiceRoom.Name}' expired due to no heartbeat received from the owner in {c_HeartbeatExpireSeconds} seconds");
						}
					}
				}

				// Tick the platform
				if (PlatformInterface != null)
				{
					PlatformInterface.Tick();
				}

				LastTickTime = DateTimeOffset.UtcNow;
				Thread.Sleep(100);
			}

			Dispose();
		}

		public void AddSDKRequest(SDKRequest request)
		{
			lock (s_Lock)
			{
				s_CurrentRequests.Add(request);
			}
		}

		public void RemoveSDKRequest(SDKRequest request)
		{
			lock (s_Lock)
			{
				s_CurrentRequests.Remove(request);
			}
		}

		public void AddVoiceRoom(VoiceRoom voiceRoom)
		{
			lock (s_Lock)
			{
				s_VoiceRooms.Add(voiceRoom);
			}
		}

		public VoiceRoom GetVoiceRoom(string roomName)
		{
			VoiceRoom voiceRoom = null;

			lock (s_Lock)
			{
				voiceRoom = s_VoiceRooms.SingleOrDefault(voiceRoom => voiceRoom.Name == roomName);
			}

			return voiceRoom;
		}

		private IHostBuilder CreateHostBuilder()
		{
			return Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults((webHostBuilder) =>
				{
					webHostBuilder.UseKestrel()
						.UseUrls($"http://{Settings.VoiceServerHost}:{Settings.VoiceServerPort}")
						.ConfigureServices((context, services) =>
						{
							services.AddControllers();
							services.AddMvc().AddNewtonsoftJson();
						})
						.Configure((app) =>
						{
							app.UseRouting();
							app.UseEndpoints(endpoints =>
							{
								endpoints.MapControllers();
							});

							var hostApplicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
							hostApplicationLifetime.ApplicationStopping.Register(() => m_CancellationTokenSource.Cancel());
						});
				})
				.UseConsoleLifetime();
		}
	}
}
