// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.SDK
{
	public class ServerMuteSDKRequest : SDKRequestWithResponse<SDKResponse>
	{
		private string RoomName { get; set; }

		private ProductUserId TargetProductUserId { get; set; }

		private bool IsMuted { get; set; }

		public ServerMuteSDKRequest(string roomName, ProductUserId targetProductUserId, bool isMuted)
		{
			RoomName = roomName;
			TargetProductUserId = targetProductUserId;
			IsMuted = isMuted;
		}

		public override void StartOperation()
		{
			base.Start();

			var setParticipantHardMuteOptions = new RTCAdmin.SetParticipantHardMuteOptions()
			{
				RoomName = RoomName,
				TargetUserId = TargetProductUserId,
				Mute = IsMuted
			};

			PlatformApplication.Instance.PlatformInterface.GetRTCAdminInterface().SetParticipantHardMute(
				ref setParticipantHardMuteOptions,
				null,
				(ref RTCAdmin.SetParticipantHardMuteCompleteCallbackInfo callbackInfo) =>
				{
					if (Common.IsOperationComplete(callbackInfo.ResultCode))
					{
						Log.WriteResult($"ServerMuteSDKRequest: SetParticipantHardMute {RoomName} {TargetProductUserId} {IsMuted}", callbackInfo.ResultCode);

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
