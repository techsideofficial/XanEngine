// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Logging;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.IntegratedPlatform;
using System.IO;

namespace Epic.OnlineServices.Samples
{
	public class EpicApplication
	{
		public static PlatformInterface Initialize(PlatformFlags platformFlags = PlatformFlags.None, bool isServer = false)
		{
			Settings.Initialize();

			string XAudio29DllPath = $"{Directory.GetCurrentDirectory()}\\xaudio2_9redist.dll";
			if (!File.Exists(XAudio29DllPath))
			{
				Log.WriteLine($"Unable to find xaudio2 library at '{XAudio29DllPath}'.", LogStyle.SuperBad);
				return null;
			}

			var initializeOptions = new InitializeOptions()
			{
				ProductName = "CSharpSamples",
				ProductVersion = "1.0.1"
			};

			Result initializeResult;
#if DEV
			initializeResult = ReservedOptions.PlatformInitialize(ref initializeOptions);
#else
			initializeResult = PlatformInterface.Initialize(ref initializeOptions);
#endif
			Log.WriteResult($"Initialize", initializeResult);

			LoggingInterface.SetLogLevel(LogCategory.AllCategories, LogLevel.Info);
			LoggingInterface.SetCallback((ref LogMessage message) => Log.WriteEpic(message));

			var createIntegratedPlatformOptionsContainerOptions = new CreateIntegratedPlatformOptionsContainerOptions();
			IntegratedPlatformOptionsContainer integratedPlatformOptionsContainer = null;
			IntegratedPlatformInterface.CreateIntegratedPlatformOptionsContainer(ref createIntegratedPlatformOptionsContainerOptions, out integratedPlatformOptionsContainer);

			var options = new WindowsOptions()
			{
				ProductId = Settings.ProductId,
				SandboxId = Settings.SandboxId,
				ClientCredentials = new ClientCredentials()
				{
					ClientId = Settings.ClientId,
					ClientSecret = Settings.ClientSecret
				},
				DeploymentId = Settings.DeploymentId,
				RTCOptions = new WindowsRTCOptions()
				{
					PlatformSpecificOptions = new WindowsRTCOptionsPlatformSpecificOptions()
					{
						XAudio29DllPath = XAudio29DllPath
					}
				},
				Flags = platformFlags,
				IsServer = isServer,
				IntegratedPlatformOptionsContainerHandle = integratedPlatformOptionsContainer
			};

			PlatformInterface platformInterface = null;
#if DEV
			platformInterface = ReservedOptions.PlatformCreate(ref options);
#else
			platformInterface = PlatformInterface.Create(ref options);
#endif

			if (platformInterface == null)
			{
				Log.WriteLine($"Failed to create platform. Ensure the relevant {typeof(Settings)} are set or passed into the application as arguments.", LogStyle.SuperBad);
			}

			if (integratedPlatformOptionsContainer != null)
			{
				integratedPlatformOptionsContainer.Release();
				integratedPlatformOptionsContainer = null;
			}

			return platformInterface;
		}
	}
}
