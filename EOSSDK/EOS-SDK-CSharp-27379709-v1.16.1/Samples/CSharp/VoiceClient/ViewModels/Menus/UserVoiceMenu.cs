// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Http;
using Epic.OnlineServices.Samples.ViewModels.UserComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Epic.OnlineServices.Samples.ViewModels.Menus
{
	public class UserVoiceMenu : UserComponentMenu<UserVoiceComponent>
	{
		private UserVoiceMenuState m_State;
		public UserVoiceMenuState State
		{
			get { return m_State; }
			set { SetProperty(ref m_State, value); }
		}

		private string m_RoomName;
		public string RoomName
		{
			get { return m_RoomName; }
			set { SetProperty(ref m_RoomName, value); }
		}

		private ObservableCollection<AudioDevice> m_InputAudioDevices;
		public ObservableCollection<AudioDevice> InputAudioDevices
		{
			get { return m_InputAudioDevices; }
			set { SetProperty(ref m_InputAudioDevices, value); }
		}

		private AudioDevice m_SelectedInputAudioDevice;
		public AudioDevice SelectedInputAudioDevice
		{
			get { return m_SelectedInputAudioDevice; }
			set
			{
				if (SetProperty(ref m_SelectedInputAudioDevice, value) && value != null)
				{
					ChangeToSelectedInputAudioDevice();
				}
			}
		}

		private ObservableCollection<AudioDevice> m_OutputAudioDevices;
		public ObservableCollection<AudioDevice> OutputAudioDevices
		{
			get { return m_OutputAudioDevices; }
			set { SetProperty(ref m_OutputAudioDevices, value); }
		}

		private AudioDevice m_SelectedOutputAudioDevice;
		public AudioDevice SelectedOutputAudioDevice
		{
			get { return m_SelectedOutputAudioDevice; }
			set
			{
				if (SetProperty(ref m_SelectedOutputAudioDevice, value) && value != null)
				{
					ChangeToSelectedOutputAudioDevice();
				}
			}
		}

		private int m_ReceivingVolume = 50;
		public int ReceivingVolume
		{
			get { return m_ReceivingVolume; }
			set {
				if (SetProperty(ref m_ReceivingVolume, value))
				{
					UpdateReceivingVolume();
				}
			}
		}

		public DelegateCommand JoinCommand { get; private set; }
		public DelegateCommand LeaveCommand { get; private set; }
		public DelegateCommand ResetParticipantVolumeCommand { get; private set; }

		private SessionResponse m_CurrentSession;

		private ulong? OnDisconnectedCallbackId;
		private ulong? OnParticipantStatusChangedCallbackId;
		private ulong? OnParticipantUpdatedCallbackId;
		private ulong? OnAudioDevicesChangedCallbackId;

		private Timer m_HeartbeatTimer;

		public UserVoiceMenu()
		{
			InputAudioDevices = new ObservableCollection<AudioDevice>();
			OutputAudioDevices = new ObservableCollection<AudioDevice>();

			JoinCommand = new DelegateCommand((parameter) => Join());
			LeaveCommand = new DelegateCommand((parameter) => LeaveRoom());
			ResetParticipantVolumeCommand = new DelegateCommand((parameter) => ResetParticipantVolume());

			m_HeartbeatTimer = new Timer()
			{
				MaxTime = 30f,
				Complete = () => Heartbeat()
			};
		}

		protected override void OnUserChanged(User oldUser, User newUser)
		{
			base.OnUserChanged(oldUser, newUser);

			if (oldUser != null)
			{
				UnsubscribeFromAudioDeviceChanges();
			}

			else if (newUser != null)
			{
				LoadInputDeviceChanges();
				LoadOutputDeviceChanges();
				SubscribeToAudioDeviceChanges();
			}
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			m_HeartbeatTimer.Update(deltaTime);
		}

		private async void Join()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null || m_CurrentSession != null)
			{
				return;
			}

			State = UserVoiceMenuState.Connecting;

			SessionResponse sessionResponse = null;

			// Create new room
			if (string.IsNullOrEmpty(RoomName))
			{
				Log.WriteLine($"Creating voice room");

				var createSessionRequest = new CreateSessionRequest()
				{
					ProductUserId = PlatformApplication.FirstLocalUser.ProductUserId.ToString(),
					Password = Settings.VoiceServerPassword
				};

				sessionResponse = await VoiceHttpClient.CreateSessionAsync(createSessionRequest);
			}
			// Join existing room
			else
			{
				Log.WriteLine($"Joining voice room {RoomName}");

				var joinSessionRequest = new JoinSessionRequest();
				sessionResponse = await VoiceHttpClient.JoinSessionAsync(joinSessionRequest, RoomName, PlatformApplication.FirstLocalUser.ProductUserId, Settings.VoiceServerPassword);
			}

			if (sessionResponse == null)
			{
				Log.WriteLine($"Join failed with unexpected response", LogStyle.Bad);
				State = UserVoiceMenuState.NotConnected;
				return;
			}
			else if (sessionResponse.JoinTokens.Count != 1)
			{
				Log.WriteLine($"Join failed as response should have exactly 1 join token but does not", LogStyle.Bad);
				State = UserVoiceMenuState.NotConnected;
				return;
			}

			var productUserId = ProductUserId.FromString(sessionResponse.JoinTokens.First().Key);
			if (productUserId != PlatformApplication.FirstLocalUser.ProductUserId)
			{
				Log.WriteLine($"Join failed as join token contains a mismatched ProductUserId", LogStyle.Bad);
				State = UserVoiceMenuState.NotConnected;
				return;
			}

			m_CurrentSession = sessionResponse;
			PlatformApplication.FirstLocalUser.GetOrAddComponent<UserVoiceComponent>().OwnerLock = m_CurrentSession.OwnerLock;

			var joinRoomOptions = new RTC.JoinRoomOptions()
			{
				LocalUserId = PlatformApplication.FirstLocalUser.ProductUserId,
				RoomName = m_CurrentSession.RoomName,
				ClientBaseUrl = m_CurrentSession.ClientBaseUrl,
				ParticipantToken = m_CurrentSession.JoinTokens.First().Value
			};

			PlatformApplication.PlatformInterface.GetRTCInterface().JoinRoom(ref joinRoomOptions, null, OnJoinRoom);
		}

		private void OnJoinRoom(ref RTC.JoinRoomCallbackInfo joinRoomCallbackInfo)
		{
			RoomName = joinRoomCallbackInfo.RoomName;
			UserComponent.RoomName = joinRoomCallbackInfo.RoomName;

			Log.WriteResult($"JoinRoom", joinRoomCallbackInfo.ResultCode);
			if (joinRoomCallbackInfo.ResultCode == Result.Success)
			{
				State = UserVoiceMenuState.Connected;
				SubscribeToRoomNotifications();
			}
			else if (Common.IsOperationComplete(joinRoomCallbackInfo.ResultCode))
			{
				State = UserVoiceMenuState.NotConnected;
			}
		}

		private void SubscribeToRoomNotifications()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null || m_CurrentSession == null)
			{
				return;
			}

			if (!OnDisconnectedCallbackId.HasValue)
			{
				var addNotifyDisconnectedOptions = new RTC.AddNotifyDisconnectedOptions()
				{
					LocalUserId = PlatformApplication.FirstLocalUser.ProductUserId,
					RoomName = m_CurrentSession.RoomName
				};

				OnDisconnectedCallbackId = PlatformApplication.PlatformInterface.GetRTCInterface().AddNotifyDisconnected(ref addNotifyDisconnectedOptions, null, OnDisconnected);
			}

			if (!OnParticipantStatusChangedCallbackId.HasValue)
			{
				var addNotifyParticipantStatusChangedOptions = new RTC.AddNotifyParticipantStatusChangedOptions()
				{
					LocalUserId = PlatformApplication.FirstLocalUser.ProductUserId,
					RoomName = m_CurrentSession.RoomName
				};

				OnParticipantStatusChangedCallbackId = PlatformApplication.PlatformInterface.GetRTCInterface().AddNotifyParticipantStatusChanged(ref addNotifyParticipantStatusChangedOptions, null, OnParticipantStatusChanged);
			}

			if (!OnParticipantUpdatedCallbackId.HasValue)
			{
				var addNotifyParticipantUpdatedOptions = new RTCAudio.AddNotifyParticipantUpdatedOptions()
				{
					LocalUserId = PlatformApplication.FirstLocalUser.ProductUserId,
					RoomName = m_CurrentSession.RoomName
				};

				OnParticipantUpdatedCallbackId = PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().AddNotifyParticipantUpdated(ref addNotifyParticipantUpdatedOptions, null, OnParticipantUpdated);
			}
		}

		private void OnParticipantUpdated(ref RTCAudio.ParticipantUpdatedCallbackInfo participantUpdatedCallbackInfo)
		{
			Log.WriteLine($"OnParticipantUpdated IsSpeaking={participantUpdatedCallbackInfo.Speaking} AudioStatus={participantUpdatedCallbackInfo.AudioStatus}");

			var participantId = participantUpdatedCallbackInfo.ParticipantId;
			var existingUser = PlatformApplication.Users.SingleOrDefault(user => user.ProductUserId == participantId);
			if (existingUser != null)
			{
				existingUser.GetOrAddComponent<UserVoiceComponent>().IsSpeaking = participantUpdatedCallbackInfo.Speaking;
				existingUser.GetOrAddComponent<UserVoiceComponent>().IsClientMuted = participantUpdatedCallbackInfo.AudioStatus != RTCAudio.RTCAudioStatus.Enabled;
				existingUser.GetOrAddComponent<UserVoiceComponent>().IsServerMuted = participantUpdatedCallbackInfo.AudioStatus == RTCAudio.RTCAudioStatus.AdminDisabled;
			}
		}

		private void OnDisconnected(ref RTC.DisconnectedCallbackInfo onDisconnectedCallbackInfo)
		{
			EndSession();
		}

		private void EndSession()
		{
			PlatformApplication.FirstLocalUser.GetOrAddComponent<UserVoiceComponent>().OwnerLock = null;

			State = UserVoiceMenuState.NotConnected;
			m_CurrentSession = null;
			UnsubscribeFromRoomNotifications();

			var remoteUsers = PlatformApplication.Users.Where(user => !user.IsLocalUser).ToArray();
			foreach (var remoteUser in remoteUsers)
			{
				PlatformApplication.Users.Remove(remoteUser);
			}
		}

		private void OnParticipantStatusChanged(ref RTC.ParticipantStatusChangedCallbackInfo onParticipantStatusChangedCallbackInfo)
		{
			if (onParticipantStatusChangedCallbackInfo.ParticipantStatus == RTC.RTCParticipantStatus.Joined)
			{
				var participantId = onParticipantStatusChangedCallbackInfo.ParticipantId;
				var user = PlatformApplication.Users.SingleOrDefault(user => user.ProductUserId == participantId);
				if (user == null)
				{
					user = new User()
					{
						ProductUserId = onParticipantStatusChangedCallbackInfo.ParticipantId
					};
					PlatformApplication.AddUser(user);

					user.UpdateEpicAccountIdFromProductUserId();
					user.GetOrAddComponent<UserVoiceComponent>().RoomName = onParticipantStatusChangedCallbackInfo.RoomName;
				}
			}
			else if (onParticipantStatusChangedCallbackInfo.ParticipantStatus == RTC.RTCParticipantStatus.Left)
			{
				var participantId = onParticipantStatusChangedCallbackInfo.ParticipantId;
				var existingUser = PlatformApplication.Users.SingleOrDefault(user => user.ProductUserId == participantId);
				if (existingUser != null && !existingUser.IsLocalUser)
				{
					PlatformApplication.RemoveUser(existingUser);
				}
			}
		}

		private async void Heartbeat()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null || m_CurrentSession == null || m_CurrentSession.OwnerLock == null)
			{
				return;
			}

			var heartbeatRequest = new HeartbeatRequest()
			{
				OwnerLock = m_CurrentSession.OwnerLock
			};

			await VoiceHttpClient.HeartbeatAsync(heartbeatRequest, RoomName);
		}

		private void UnsubscribeFromRoomNotifications()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null)
			{
				return;
			}

			if (OnDisconnectedCallbackId.HasValue)
			{
				PlatformApplication.PlatformInterface.GetRTCInterface().RemoveNotifyDisconnected(OnDisconnectedCallbackId.Value);
				OnDisconnectedCallbackId = null;
			}

			if (OnParticipantStatusChangedCallbackId.HasValue)
			{
				PlatformApplication.PlatformInterface.GetRTCInterface().RemoveNotifyParticipantStatusChanged(OnParticipantStatusChangedCallbackId.Value);
				OnParticipantStatusChangedCallbackId = null;
			}

			if (OnParticipantUpdatedCallbackId.HasValue)
			{
				PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().RemoveNotifyParticipantUpdated(OnParticipantUpdatedCallbackId.Value);
				OnParticipantUpdatedCallbackId = null;
			}
		}

		private void LeaveRoom()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null || m_CurrentSession == null)
			{
				return;
			}

			State = UserVoiceMenuState.Disconnecting;

			var leaveRoomOptions = new RTC.LeaveRoomOptions()
			{
				LocalUserId = UserComponent.User.ProductUserId,
				RoomName = m_CurrentSession.RoomName,
			};

			PlatformApplication.PlatformInterface.GetRTCInterface().LeaveRoom(ref leaveRoomOptions, null, OnLeaveRoom);
		}

		private void OnLeaveRoom(ref RTC.LeaveRoomCallbackInfo leaveRoomCallbackInfo)
		{
			Log.WriteResult($"OnLeaveRoom", leaveRoomCallbackInfo.ResultCode);
			EndSession();
		}

		private static void UpdateAudioDevices(ref ObservableCollection<AudioDevice> existingAudioDevices, IEnumerable<AudioDevice> newAudioDevices, AudioDevice selectedAudioDevice, Action<AudioDevice> selectAudioDeviceAction)
		{
			IEnumerable<AudioDevice> localExistingAudioDevices = existingAudioDevices;
			var inputAudioDevicesToAdd = newAudioDevices.Where(newInputAudioDevice => !localExistingAudioDevices.Where(oldInputAudioDevice => newInputAudioDevice.Id == oldInputAudioDevice.Id).Any());
			var inputAudioDevicesToRemove = existingAudioDevices.Where(oldInputAudioDevice => !newAudioDevices.Where(newInputAudioDevice => oldInputAudioDevice.Id == newInputAudioDevice.Id).Any());
			var inputAudioDevicesToUpdate = existingAudioDevices.Except(inputAudioDevicesToRemove);

			// Select the default device if needbe
			bool selectedInputAudioDeviceWillBeRemoved = selectedAudioDevice != null && inputAudioDevicesToRemove.Contains(selectedAudioDevice);
			bool noSelectedInputAudioDevice = selectedAudioDevice == null;

			// Perform the changes
			foreach (var inputAudioDeviceToAdd in inputAudioDevicesToAdd)
			{
				existingAudioDevices.Add(inputAudioDeviceToAdd);
			}

			foreach (var inputAudioDeviceToRemove in inputAudioDevicesToRemove.ToArray())
			{
				existingAudioDevices.Remove(inputAudioDeviceToRemove);
			}

			foreach (var inputAudioDeviceToUpdate in inputAudioDevicesToUpdate)
			{
				var updatedOutputAudioDevice = newAudioDevices.SingleOrDefault(newOutputAudioDevice => inputAudioDeviceToUpdate.Id == newOutputAudioDevice.Id);
				if (updatedOutputAudioDevice != null)
				{
					inputAudioDeviceToUpdate.Name = updatedOutputAudioDevice.Name;
					inputAudioDeviceToUpdate.IsDefault = updatedOutputAudioDevice.IsDefault;
				}
			}

			if (noSelectedInputAudioDevice || selectedInputAudioDeviceWillBeRemoved)
			{
				var defaultInputAudioDevice = existingAudioDevices.SingleOrDefault(newInputAudioDevice => newInputAudioDevice.IsDefault);
				if (defaultInputAudioDevice != null)
				{
					selectAudioDeviceAction(defaultInputAudioDevice);
				}
			}
		}

		private void LoadInputDeviceChanges()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null)
			{
				return;
			}

			List<AudioDevice> newInputAudioDevices = new List<AudioDevice>();
			var getAudioInputDevicesCountOptions = new RTCAudio.GetAudioInputDevicesCountOptions();
			uint audioInputDevicesCount = PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().GetAudioInputDevicesCount(ref getAudioInputDevicesCountOptions);
			for (uint index = 0; index < audioInputDevicesCount; ++index)
			{
				var getAudioInputDeviceByIndexOptions = new RTCAudio.GetAudioInputDeviceByIndexOptions()
				{
					DeviceInfoIndex = index
				};

				var audioInputDeviceInfo = PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().GetAudioInputDeviceByIndex(ref getAudioInputDeviceByIndexOptions);
				if (audioInputDeviceInfo == null)
				{
					Log.WriteLine($"GetAudioInputDeviceByIndex failed at index {index}");
				}
				else
				{
					Log.WriteLine($"GetAudioInputDeviceByIndex {audioInputDeviceInfo.Value.DeviceName}");
					var newAudioDevice = new AudioDevice()
					{
						Id = audioInputDeviceInfo.Value.DeviceId,
						Name = audioInputDeviceInfo.Value.DeviceName,
						IsDefault = audioInputDeviceInfo.Value.DefaultDevice
					};

					newInputAudioDevices.Add(newAudioDevice);
				}
			}

			UpdateAudioDevices(ref m_InputAudioDevices, newInputAudioDevices, SelectedInputAudioDevice, (AudioDevice selectedAudioDevice) =>
			{
				SelectedInputAudioDevice = selectedAudioDevice;
			});
		}

		private void LoadOutputDeviceChanges()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null)
			{
				return;
			}

			List<AudioDevice> newOutputAudioDevices = new List<AudioDevice>();
			var getAudioOutputDevicesCountOptions = new RTCAudio.GetAudioOutputDevicesCountOptions();
			uint audioOutputDevicesCount = PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().GetAudioOutputDevicesCount(ref getAudioOutputDevicesCountOptions);
			for (uint index = 0; index < audioOutputDevicesCount; ++index)
			{
				var getAudioOutputDeviceByIndexOptions = new RTCAudio.GetAudioOutputDeviceByIndexOptions()
				{
					DeviceInfoIndex = index
				};

				var audioOutputDeviceInfo = PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().GetAudioOutputDeviceByIndex(ref getAudioOutputDeviceByIndexOptions);
				if (audioOutputDeviceInfo == null)
				{
					Log.WriteLine($"GetAudioOutputDeviceByIndex failed at index {index}");
				}
				else
				{
					Log.WriteLine($"GetAudioOutputDeviceByIndex {audioOutputDeviceInfo.Value.DeviceName}");
					var newAudioDevice = new AudioDevice()
					{
						Id = audioOutputDeviceInfo.Value.DeviceId,
						Name = audioOutputDeviceInfo.Value.DeviceName,
						IsDefault = audioOutputDeviceInfo.Value.DefaultDevice
					};

					newOutputAudioDevices.Add(newAudioDevice);
				}
			}

			UpdateAudioDevices(ref m_OutputAudioDevices, newOutputAudioDevices, SelectedOutputAudioDevice, (AudioDevice selectedAudioDevice) =>
			{
				SelectedOutputAudioDevice = selectedAudioDevice;
			});
		}

		private void SubscribeToAudioDeviceChanges()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null)
			{
				return;
			}

			if (!OnAudioDevicesChangedCallbackId.HasValue)
			{
				var addNotifyAudioDevicesChangedOptions = new RTCAudio.AddNotifyAudioDevicesChangedOptions();
				OnAudioDevicesChangedCallbackId = PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().AddNotifyAudioDevicesChanged(ref addNotifyAudioDevicesChangedOptions, null, OnAudioDevicesChanged);
			}
		}

		private void OnAudioDevicesChanged(ref RTCAudio.AudioDevicesChangedCallbackInfo audioDevicesChangedCallbackInfo)
		{
			LoadInputDeviceChanges();
			LoadOutputDeviceChanges();
		}

		private void UnsubscribeFromAudioDeviceChanges()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null)
			{
				return;
			}

			if (OnAudioDevicesChangedCallbackId.HasValue)
			{
				PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().RemoveNotifyAudioDevicesChanged(OnAudioDevicesChangedCallbackId.Value);
				OnAudioDevicesChangedCallbackId = null;
			}
		}

		private void ChangeToSelectedInputAudioDevice()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null)
			{
				return;
			}

			var setAudioInputSettingsOptions = new RTCAudio.SetAudioInputSettingsOptions()
			{
				LocalUserId = UserComponent.User.ProductUserId,
				DeviceId = SelectedInputAudioDevice.Id,
				Volume = 50.0f
			};

			var result = PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().SetAudioInputSettings(ref setAudioInputSettingsOptions);
			Log.WriteResult($"SetAudioInputSettings", result);
		}

		private void ChangeToSelectedOutputAudioDevice()
		{
			if (PlatformApplication == null || PlatformApplication.PlatformInterface == null)
			{
				return;
			}

			var setAudioOutputSettingsOptions = new RTCAudio.SetAudioOutputSettingsOptions()
			{
				LocalUserId = UserComponent.User.ProductUserId,
				DeviceId = SelectedOutputAudioDevice.Id,
				Volume = 50.0f
			};

			var result = PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().SetAudioOutputSettings(ref setAudioOutputSettingsOptions);
			Log.WriteResult($"SetAudioOutputSettings", result);
		}

		private void UpdateReceivingVolume()
		{
			var updateReceivingVolumeOptions = new RTCAudio.UpdateReceivingVolumeOptions()
			{
				LocalUserId = User.PlatformApplication.FirstLocalUser.ProductUserId,
				RoomName = RoomName,
				Volume = m_ReceivingVolume
			};

			User.PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().UpdateReceivingVolume(ref updateReceivingVolumeOptions, null, OnUpdateReceivingVolume);
		}

		private void OnUpdateReceivingVolume(ref RTCAudio.UpdateReceivingVolumeCallbackInfo updateReceivingVolumeCallbackInfo)
		{
			Log.WriteResult("OnUpdateReceivingVolume", updateReceivingVolumeCallbackInfo.ResultCode);
		}

		private void ResetParticipantVolume()
		{
			var updateParticipantVolumeOptions = new RTCAudio.UpdateParticipantVolumeOptions()
			{
				LocalUserId = User.PlatformApplication.FirstLocalUser.ProductUserId,
				RoomName = RoomName,
				ParticipantId = null,
				Volume = 50
			};

			User.PlatformApplication.PlatformInterface.GetRTCInterface().GetAudioInterface().UpdateParticipantVolume(ref updateParticipantVolumeOptions, null, OnResetParticipantVolume);
		}
		private void OnResetParticipantVolume(ref RTCAudio.UpdateParticipantVolumeCallbackInfo updateParticipantVolumeCallbackInfo)
		{
			Log.WriteResult("OnResetParticipantVolume", updateParticipantVolumeCallbackInfo.ResultCode);
		}
	}
}