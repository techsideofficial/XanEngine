// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Samples;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Epic.OnlineServices.Http
{
	public static class VoiceHttpClient
	{
		private static HttpClient s_HttpClient = new HttpClient();

		private static string VoiceServerUrl => $"http://{Settings.VoiceServerHost}:{Settings.VoiceServerPort}";

		private static async Task<TResponse> VoicePostAsync<TRequest, TResponse>(string url, TRequest request)
		{
			try
			{
				string jsonContent = JsonConvert.SerializeObject(request);

				Log.WriteLine($"VoicePostAsync {url} payload {jsonContent}");

				var stringContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
				var httpResponseMessage = await s_HttpClient.PostAsync(url, stringContent);
				if (!httpResponseMessage.IsSuccessStatusCode)
				{
					var errorResponseString = await httpResponseMessage.Content.ReadAsStringAsync();
					var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorResponseString);
					if (errorResponse != null)
					{
						Log.WriteLine($"VoicePostAsync failed with status code {(int)httpResponseMessage.StatusCode}: {httpResponseMessage.ReasonPhrase}: {errorResponse.Description}", LogStyle.Bad);
					}
					else
					{
						Log.WriteLine($"VoicePostAsync failed with status code {(int)httpResponseMessage.StatusCode}: {httpResponseMessage.ReasonPhrase}", LogStyle.Bad);
					}

					return default(TResponse);
				}

				var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
				var response = JsonConvert.DeserializeObject<TResponse>(responseString);
				return response;
			}
			catch (Exception ex)
			{
				Log.WriteException(ex);
			}

			return default(TResponse);
		}

		public static async Task<SessionResponse> CreateSessionAsync(CreateSessionRequest request)
		{
			string url = $"{VoiceServerUrl}/session";
			var response = await VoicePostAsync<CreateSessionRequest, SessionResponse>(url, request);
			return response;
		}

		public static async Task<SessionResponse> JoinSessionAsync(JoinSessionRequest request, string roomName, ProductUserId productUserId, string password)
		{
			string url = $"{VoiceServerUrl}/session/{roomName}/join/{productUserId}";

			if (!string.IsNullOrEmpty(password))
			{
				url = $"{url}?password={password}";
			}

			var response = await VoicePostAsync<JoinSessionRequest, SessionResponse>(url, request);
			return response;
		}

		public static async Task<ServerMuteResponse> ServerMuteAsync(ServerMuteRequest request, string roomName, ProductUserId productUserId)
		{
			string url = $"{VoiceServerUrl}/session/{roomName}/mute/{productUserId}";
			var response = await VoicePostAsync<ServerMuteRequest, ServerMuteResponse>(url, request);
			return response;
		}

		public static async Task<KickResponse> KickAsync(KickRequest request, string roomName, ProductUserId productUserId)
		{
			string url = $"{VoiceServerUrl}/session/{roomName}/kick/{productUserId}";
			var response = await VoicePostAsync<KickRequest, KickResponse>(url, request);
			return response;
		}

		public static async Task<HeartbeatResponse> HeartbeatAsync(HeartbeatRequest request, string roomName)
		{
			string url = $"{VoiceServerUrl}/session/{roomName}/heartbeat";
			var response = await VoicePostAsync<HeartbeatRequest, HeartbeatResponse>(url, request);
			return response;
		}
	}
}
