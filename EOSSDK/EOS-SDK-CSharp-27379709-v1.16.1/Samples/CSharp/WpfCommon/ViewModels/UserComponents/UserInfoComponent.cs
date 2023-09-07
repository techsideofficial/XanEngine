// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.UserInfo;

namespace Epic.OnlineServices.Samples.ViewModels.UserComponents
{
	public class UserInfoComponent : UserComponent
	{
		private UpdateState m_UpdateState;
		public UpdateState UpdateState
		{
			get { return m_UpdateState; }
			set { SetProperty(ref m_UpdateState, value); }
		}

		private string m_Name;
		public string Name
		{
			get { return m_Name; }
			set { SetProperty(ref m_Name, value); }
		}

		private bool m_HasUpdated;

		public UserInfoComponent()
		{
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			if (!CanUsePlatform || UpdateState != UpdateState.Done || m_HasUpdated || User.PlatformApplication.FirstLocalUser == null || User.PlatformApplication.FirstLocalUser.EpicAccountId == null || User.EpicAccountId == null)
			{
				return;
			}

			UpdateInfo();
		}

		private void UpdateInfo()
		{
			UpdateState = UpdateState.InProgress;

			var queryUserInfoOptions = new QueryUserInfoOptions()
			{
				LocalUserId = User.PlatformApplication.FirstLocalUser.EpicAccountId,
				TargetUserId = User.EpicAccountId
			};

			User.PlatformApplication.PlatformInterface.GetUserInfoInterface().QueryUserInfo(ref queryUserInfoOptions, null, OnQueryUserInfo);
		}

		private void OnQueryUserInfo(ref QueryUserInfoCallbackInfo queryUserInfoCallbackInfo)
		{
			Log.WriteResult($"OnQueryUserInfo", queryUserInfoCallbackInfo.ResultCode);

			if (queryUserInfoCallbackInfo.ResultCode == Result.Success)
			{
				m_HasUpdated = true;

				var copyUserInfoOptions = new CopyUserInfoOptions()
				{
					LocalUserId = queryUserInfoCallbackInfo.LocalUserId,
					TargetUserId = queryUserInfoCallbackInfo.TargetUserId
				};

				var result = User.PlatformApplication.PlatformInterface.GetUserInfoInterface().CopyUserInfo(ref copyUserInfoOptions, out var userInfoData);
				Log.WriteResult($"CopyUserInfo", result);

				if (result == Result.Success && userInfoData.HasValue)
				{
					Name = userInfoData.Value.DisplayName;
				}
			}

			if (Common.IsOperationComplete(queryUserInfoCallbackInfo.ResultCode))
			{
				UpdateState = UpdateState.Done;
			}
		}
	}
}
