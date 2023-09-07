// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Http;

namespace Epic.OnlineServices.Samples.ViewModels.UserComponents
{
	public class UserVoiceComponent : UserComponent
	{
		private string m_RoomName;
		public string RoomName
		{
			get { return m_RoomName; }
			set { SetProperty(ref m_RoomName, value); }
		}

		private string m_OwnerLock;
		public string OwnerLock
		{
			get { return m_OwnerLock; }
			set
			{
				if (SetProperty(ref m_OwnerLock, value))
				{
					NotifyPropertyChanged(nameof(IsAdmin));
				}
			}
		}

		public bool IsAdmin
		{
			get { return !string.IsNullOrEmpty(OwnerLock); }
		}

		private bool m_IsClientMuted;
		public bool IsClientMuted
		{
			get { return m_IsClientMuted; }
			set { SetProperty(ref m_IsClientMuted, value); }
		}

		private bool m_IsServerMuted;
		public bool IsServerMuted
		{
			get { return m_IsServerMuted; }
			set { SetProperty(ref m_IsServerMuted, value); }
		}

		private bool m_IsSpeaking;
		public bool IsSpeaking
		{
			get { return m_IsSpeaking; }
			set { SetProperty(ref m_IsSpeaking, value); }
		}

		private int m_ParticipantVolume = 50;
		public int ParticipantVolume
		{
			get { return m_ParticipantVolume; }
			set
			{
				if (SetProperty(ref m_ParticipantVolume, value))
				{
					UpdateParticipantVolume();
				}
			}
		}

		public DelegateCommand ToggleClientMuteCommand { get; private set; }
		public DelegateCommand ToggleServerMuteCommand { get; private set; }
		public DelegateCommand ServerKickCommand { get; private set; }

		public UserVoiceComponent()
		{
			ToggleClientMuteCommand = new DelegateCommand((parameter) => ToggleClientMute());
			ToggleServerMuteCommand = new DelegateCommand((parameter) => ToggleServerMute());
			ServerKickCommand = new DelegateCommand((parameter) => ServerKick());
		}

		private void ToggleClientMute()
		{
			if (User.IsLocalUser)
			{
				var updateSendingOptions = new RTCAudio.UpdateSendingOptions()
				{
					LocalUserId = User.ProductUserId,
					RoomName = RoomName,
					AudioStatus = IsClientMuted ? RTCAudio.RTCAudioStatus.Enabled : RTCAudio.RTCAudioStatus.Disabled
				};

				User.PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().UpdateSending(ref updateSendingOptions, null, OnUpdateSending);
			}
			else
			{
				var updateReceivingOptions = new RTCAudio.UpdateReceivingOptions()
				{
					LocalUserId = User.PlatformApplication.FirstLocalUser.ProductUserId,
					RoomName = RoomName,
					ParticipantId = User.ProductUserId,
					AudioEnabled = IsClientMuted
				};

				User.PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().UpdateReceiving(ref updateReceivingOptions, null, OnUpdateReceiving);

				IsClientMuted = !IsClientMuted;
			}
		}

		private void OnUpdateSending(ref RTCAudio.UpdateSendingCallbackInfo updateSendingCallbackInfo)
		{
			Log.WriteResult("OnUpdateSending", updateSendingCallbackInfo.ResultCode);
		}

		private void OnUpdateReceiving(ref RTCAudio.UpdateReceivingCallbackInfo updateReceivingCallbackInfo)
		{
			Log.WriteResult("OnUpdateReceiving", updateReceivingCallbackInfo.ResultCode);
		}

		private async void ToggleServerMute()
		{
			var serverMuteRequest = new ServerMuteRequest()
			{
				OwnerLock = User.PlatformApplication.FirstLocalUser.GetOrAddComponent<UserVoiceComponent>().OwnerLock,
				IsMuted = !IsServerMuted
			};

			await VoiceHttpClient.ServerMuteAsync(serverMuteRequest, RoomName, User.ProductUserId);
		}

		private async void ServerKick()
		{
			var kickRequest = new KickRequest()
			{
				OwnerLock = User.PlatformApplication.FirstLocalUser.GetOrAddComponent<UserVoiceComponent>().OwnerLock
			};

			await VoiceHttpClient.KickAsync(kickRequest, RoomName, User.ProductUserId);
		}

		private void UpdateParticipantVolume()
		{
			if (!User.IsLocalUser)
			{
				var updateParticipantVolumeOptions = new RTCAudio.UpdateParticipantVolumeOptions()
				{
					LocalUserId = User.PlatformApplication.FirstLocalUser.ProductUserId,
					RoomName = RoomName,
					ParticipantId = User.ProductUserId, // can be null if we want reset volume of all participants in the room
					Volume = m_ParticipantVolume
				};

				User.PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().UpdateParticipantVolume(ref updateParticipantVolumeOptions, null, OnUpdateParticipantVolume);
			}
		}

		private void OnUpdateParticipantVolume(ref RTCAudio.UpdateParticipantVolumeCallbackInfo updateParticipantVolumeCallbackInfo)
		{
			Log.WriteResult("OnUpdateParticipantVolume", updateParticipantVolumeCallbackInfo.ResultCode);
		}
	}
}
