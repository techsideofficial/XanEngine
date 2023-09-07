// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Http;
using Epic.OnlineServices.Samples.SDK;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Epic.OnlineServices.Samples
{
	[ApiController]
	public class VoiceRoomController : ControllerBase
	{
		private string GenerateRandomId(int length)
		{
			var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var stringChars = new char[length];
			var random = new Random();

			for (int index = 0; index < stringChars.Length; index++)
			{
				stringChars[index] = chars[random.Next(chars.Length)];
			}

			return new string(stringChars);
		}

		private IActionResult GetErrorResult(int statusCode, string description)
		{
			return StatusCode(statusCode, new ErrorResponse()
			{
				Description = description
			});
		}

		private IActionResult GetInvalidPlatformResult()
		{
			return GetErrorResult(500, $"The operation cannot complete because the EOS platform is invalid.");
		}

		private IActionResult GetOperationTimeoutResult(float timeoutSeconds)
		{
			return GetErrorResult(408, $"The operation timed out because it exceeded the limit of {timeoutSeconds} seconds.");
		}

		private IActionResult GetEOSOperationFailureResult(Result result)
		{
			return GetErrorResult(502, $"An EOS call failed with result {result}.");
		}

		private IActionResult GetIncorrectPasswordResult()
		{
			return GetErrorResult(403, $"The given password is incorrect.");
		}

		private IActionResult GetIncorrectOwnerLockResult()
		{
			return GetErrorResult(403, $"The given owner lock is incorrect. You must be the room owner to use this operation.");
		}

		private IActionResult GetRoomNotFoundResult(string roomName)
		{
			return GetErrorResult(404, $"Unable to find a room with a name of {roomName}.");
		}

		private IActionResult GetInputNotProvidedResult(string inputName)
		{
			return GetErrorResult(400, $"A required input was not provided: {inputName}.");
		}

		private IActionResult GetBannedResult()
		{
			return GetErrorResult(403, $"The user has been banned from the room.");
		}

		private async Task<(IActionResult, TSDKResponse)> WaitForResponseAsync<TSDKResponse>(SDKRequestWithResponse<TSDKResponse> sdkRequest, float timeoutSeconds = 30)
			where TSDKResponse : SDKResponse
		{
			DateTimeOffset startTime = DateTimeOffset.UtcNow;
			DateTimeOffset timeoutTime = startTime.AddSeconds(timeoutSeconds);

			PlatformApplication.Instance.AddSDKRequest(sdkRequest);

			IActionResult result;
			while (true)
			{
				// Platform not created
				if (PlatformApplication.Instance.PlatformInterface == null)
				{
					result = GetInvalidPlatformResult();
					break;
				}

				// Ok
				if (sdkRequest.Response != null && sdkRequest.Response.Result == Result.Success)
				{
					result = Ok();
					break;
				}

				// Operation failed
				if (sdkRequest.Response != null && Common.IsOperationComplete(sdkRequest.Response.Result))
				{
					result = GetEOSOperationFailureResult(sdkRequest.Response.Result);
					break;
				}

				// Operation timed out
				if (timeoutTime < PlatformApplication.Instance.LastTickTime)
				{
					result = GetOperationTimeoutResult(timeoutSeconds);
					break;
				}

				await Task.Delay(100);
			}

			PlatformApplication.Instance.RemoveSDKRequest(sdkRequest);

			return (result, sdkRequest.Response);
		}

		[HttpPost]
		[Route("session")]
		public async Task<IActionResult> CreateRoomAsync([FromBody] CreateSessionRequest request)
		{
			// Missing required input
			if (request == null)
			{
				return GetInputNotProvidedResult("CreateSessionRequest");
			}

			if (request.ProductUserId == null)
			{
				return GetInputNotProvidedResult("ProductUserId");
			}

			// Incorrect password
			if (request.Password != Settings.VoiceServerPassword)
			{
				return GetIncorrectPasswordResult();
			}

			// Platform not created
			if (PlatformApplication.Instance.PlatformInterface == null)
			{
				return GetInvalidPlatformResult();
			}

			string roomName = GenerateRandomId(16);

			var (result, sdkResponse) = await WaitForResponseAsync(new JoinOrCreateRoomSDKRequest(roomName, ProductUserId.FromString(request.ProductUserId)));
			if (result is OkResult)
			{
				string ownerLock = GenerateRandomId(8);

				VoiceRoom voiceRoom = new VoiceRoom(sdkResponse.RoomName, ownerLock, sdkResponse.ClientBaseUrl);
				PlatformApplication.Instance.AddVoiceRoom(voiceRoom);

				return Ok(new SessionResponse()
				{
					RoomName = sdkResponse.RoomName,
					ClientBaseUrl = sdkResponse.ClientBaseUrl,
					OwnerLock = voiceRoom.OwnerLock,
					JoinTokens = sdkResponse.ProductUserIdTokens.ToDictionary(pair => pair.Key.ToString().ToString(), pair => pair.Value)
				});
			}

			return result;
		}

		[HttpPost]
		[Route("session/{roomName}/join/{productUserIdString}")]
		public async Task<IActionResult> JoinRoomAsync([FromBody] JoinSessionRequest request, string roomName, string productUserIdString, string password)
		{
			// Missing required input
			if (roomName == null)
			{
				return GetInputNotProvidedResult("Room Name");
			}

			if (productUserIdString == null)
			{
				return GetInputNotProvidedResult("ProductUserId");
			}

			// Incorrect password
			if (password != Settings.VoiceServerPassword)
			{
				return GetIncorrectPasswordResult();
			}

			// Room doesn't exist
			var voiceRoom = PlatformApplication.Instance.GetVoiceRoom(roomName);
			if (voiceRoom == null)
			{
				return GetRoomNotFoundResult(roomName);
			}

			// User is banned
			var productUserId = ProductUserId.FromString(productUserIdString);
			if (voiceRoom.IsBanned(productUserId))
			{
				return GetBannedResult();
			}

			// Platform not created
			if (PlatformApplication.Instance.PlatformInterface == null)
			{
				return GetInvalidPlatformResult();
			}

			var (result, sdkResponse) = await WaitForResponseAsync(new JoinOrCreateRoomSDKRequest(roomName, productUserId));
			if (result is OkResult)
			{
				return Ok(new SessionResponse()
				{
					RoomName = sdkResponse.RoomName,
					ClientBaseUrl = sdkResponse.ClientBaseUrl,
					JoinTokens = sdkResponse.ProductUserIdTokens.ToDictionary(pair => pair.Key.ToString().ToString(), pair => pair.Value)
				});
			}

			return result;
		}

		[HttpPost]
		[Route("session/{roomName}/mute/{productUserIdString}")]
		public async Task<IActionResult> ServerMuteAsync([FromBody] ServerMuteRequest request, string roomName, string productUserIdString)
		{
			// Missing required input
			if (request == null)
			{
				return GetInputNotProvidedResult("ServerMuteRequest");
			}

			if (request.OwnerLock == null)
			{
				return GetInputNotProvidedResult("Owner Lock");
			}

			if (roomName == null)
			{
				return GetInputNotProvidedResult("Room Name");
			}

			if (productUserIdString == null)
			{
				return GetInputNotProvidedResult("ProductUserId");
			}

			// Room doesn't exist
			var voiceRoom = PlatformApplication.Instance.GetVoiceRoom(roomName);
			if (voiceRoom == null)
			{
				return GetRoomNotFoundResult(roomName);
			}

			// Incorrect owner lock
			if (voiceRoom.OwnerLock != request.OwnerLock)
			{
				return GetIncorrectOwnerLockResult();
			}

			// Platform not created
			if (PlatformApplication.Instance.PlatformInterface == null)
			{
				return GetInvalidPlatformResult();
			}

			var (result, sdkResponse) = await WaitForResponseAsync(new ServerMuteSDKRequest(roomName, ProductUserId.FromString(productUserIdString), request.IsMuted));
			if (result is OkResult)
			{
				return Ok(new KickResponse());
			}

			return result;
		}

		[HttpPost]
		[Route("session/{roomName}/kick/{productUserIdString}")]
		public async Task<IActionResult> KickAsync([FromBody] KickRequest request, string roomName, string productUserIdString)
		{
			// Missing required input
			if (request == null)
			{
				return GetInputNotProvidedResult("ServerMuteRequest");
			}

			if (request.OwnerLock == null)
			{
				return GetInputNotProvidedResult("Owner Lock");
			}

			if (roomName == null)
			{
				return GetInputNotProvidedResult("Room Name");
			}

			if (productUserIdString == null)
			{
				return GetInputNotProvidedResult("ProductUserId");
			}

			// Room doesn't exist
			var voiceRoom = PlatformApplication.Instance.GetVoiceRoom(roomName);
			if (voiceRoom == null)
			{
				return GetRoomNotFoundResult(roomName);
			}

			// Incorrect owner lock
			if (voiceRoom.OwnerLock != request.OwnerLock)
			{
				return GetIncorrectOwnerLockResult();
			}

			// Platform not created
			if (PlatformApplication.Instance.PlatformInterface == null)
			{
				return GetInvalidPlatformResult();
			}

			var productUserId = ProductUserId.FromString(productUserIdString);

			var (result, sdkResponse) = await WaitForResponseAsync(new KickSDKRequest(roomName, productUserId));
			if (result is OkResult)
			{
				voiceRoom.Ban(productUserId);
				return Ok(new KickResponse());
			}

			return result;
		}

		[HttpPost]
		[Route("session/{roomName}/heartbeat")]
		public IActionResult Heartbeat([FromBody] HeartbeatRequest request, string roomName)
		{
			// Missing required input
			if (request == null)
			{
				return GetInputNotProvidedResult("ServerMuteRequest");
			}

			if (request.OwnerLock == null)
			{
				return GetInputNotProvidedResult("ServerMuteRequest: Owner Lock");
			}

			if (roomName == null)
			{
				return GetInputNotProvidedResult("ServerMuteRequest: Room Name");
			}

			// Room doesn't exist
			var voiceRoom = PlatformApplication.Instance.GetVoiceRoom(roomName);
			if (voiceRoom == null)
			{
				return GetRoomNotFoundResult(roomName);
			}

			// Incorrect owner lock
			if (voiceRoom.OwnerLock != request.OwnerLock)
			{
				return GetIncorrectOwnerLockResult();
			}

			// Platform not created
			if (PlatformApplication.Instance.PlatformInterface == null)
			{
				return GetInvalidPlatformResult();
			}

			voiceRoom.Heartbeat();
			return Ok(new HeartbeatResponse());
		}
	}
}
