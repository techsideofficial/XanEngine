// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.SDK
{
	public class JoinOrCreateRoomSDKRequest : SDKRequestWithResponse<JoinOrCreateRoomSDKResponse>
	{
		private string RoomName { get; set; }

		private ProductUserId TargetProductUserId { get; set; }

		public JoinOrCreateRoomSDKRequest(string roomName, ProductUserId targetProductUserId)
		{
			RoomName = roomName;
			TargetProductUserId = targetProductUserId;
		}

		public override void StartOperation()
		{
			base.Start();

			var queryJoinRoomTokenOptions = new RTCAdmin.QueryJoinRoomTokenOptions()
			{
				RoomName = RoomName,
				TargetUserIds = new ProductUserId[] { TargetProductUserId }
			};
			PlatformApplication.Instance.PlatformInterface.GetRTCAdminInterface().QueryJoinRoomToken(
				ref queryJoinRoomTokenOptions,
				null,
				(ref RTCAdmin.QueryJoinRoomTokenCompleteCallbackInfo callbackInfo) =>
				{
					Log.WriteResult($"JoinOrCreateRoomSDKRequest: QueryJoinRoomToken {RoomName}", callbackInfo.ResultCode);

					if (callbackInfo.ResultCode == Result.Success)
					{
						var response = new JoinOrCreateRoomSDKResponse()
						{
							Result = callbackInfo.ResultCode,
							RoomName = callbackInfo.RoomName,
							ClientBaseUrl = callbackInfo.ClientBaseUrl
						};

						var copyUserTokenByIndexOptions = new RTCAdmin.CopyUserTokenByIndexOptions();
						copyUserTokenByIndexOptions.QueryId = callbackInfo.QueryId;
						for (copyUserTokenByIndexOptions.UserTokenIndex = 0; copyUserTokenByIndexOptions.UserTokenIndex < callbackInfo.TokenCount; ++copyUserTokenByIndexOptions.UserTokenIndex)
						{
							var result = PlatformApplication.Instance.PlatformInterface.GetRTCAdminInterface().CopyUserTokenByIndex(ref copyUserTokenByIndexOptions, out var userToken);
							if (result == Result.Success && userToken.HasValue)
							{
								// Only return the token for the requested user
								if (TargetProductUserId == userToken.Value.ProductUserId)
								{
									response.ProductUserIdTokens.Add(userToken.Value.ProductUserId, userToken.Value.Token);
								}
							}
							else
							{
								Log.WriteResult($"JoinOrCreateRoomSDKRequest: CopyUserTokenByIndex {copyUserTokenByIndexOptions.UserTokenIndex}", result);
							}
						}

						Complete(response);
					}
					else if (Common.IsOperationComplete(callbackInfo.ResultCode))
					{
						var response = new JoinOrCreateRoomSDKResponse()
						{
							Result = callbackInfo.ResultCode
						};

						Complete(response);
					}
				}
			);
		}
	}
}
