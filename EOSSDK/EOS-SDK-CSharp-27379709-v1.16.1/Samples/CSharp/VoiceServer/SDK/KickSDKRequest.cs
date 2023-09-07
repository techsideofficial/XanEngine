// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.SDK
{
	public class KickSDKRequest : SDKRequestWithResponse<SDKResponse>
	{
		private string RoomName { get; set; }

		private ProductUserId TargetProductUserId { get; set; }

		public KickSDKRequest(string roomName, ProductUserId targetProductUserId)
		{
			RoomName = roomName;
			TargetProductUserId = targetProductUserId;
		}

		public override void StartOperation()
		{
			base.Start();

			var kickOptions = new RTCAdmin.KickOptions()
			{
				RoomName = RoomName,
				TargetUserId = TargetProductUserId
			};

			PlatformApplication.Instance.PlatformInterface.GetRTCAdminInterface().Kick(
				ref kickOptions,
				null,
				(ref RTCAdmin.KickCompleteCallbackInfo callbackInfo) =>
				{
					if (Common.IsOperationComplete(callbackInfo.ResultCode))
					{
						Log.WriteResult($"KickSDKRequest: Kick {RoomName} {TargetProductUserId}", callbackInfo.ResultCode);

						SDKResponse response = new SDKResponse()
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
