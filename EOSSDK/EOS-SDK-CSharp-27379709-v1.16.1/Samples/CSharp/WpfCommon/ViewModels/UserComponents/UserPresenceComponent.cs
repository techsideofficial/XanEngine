// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Presence;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Epic.OnlineServices.Samples.ViewModels.UserComponents
{
	public class UserPresenceComponent : UserComponent
	{
		private UpdateState m_UpdateState;
		public UpdateState UpdateState
		{
			get { return m_UpdateState; }
			set { SetProperty(ref m_UpdateState, value); }
		}

		private Status m_Status;
		public Status Status
		{
			get { return m_Status; }
			set { SetProperty(ref m_Status, value); }
		}

		private string m_ProductIdText;
		public string ProductIdText
		{
			get { return m_ProductIdText; }
			set { SetProperty(ref m_ProductIdText, value); }
		}

		private string m_ProductVersionText;
		public string ProductVersionText
		{
			get { return m_ProductVersionText; }
			set { SetProperty(ref m_ProductVersionText, value); }
		}

		private string m_PlatformText;
		public string PlatformText
		{
			get { return m_PlatformText; }
			set { SetProperty(ref m_PlatformText, value); }
		}

		private string m_RichText;
		public string RichText
		{
			get { return m_RichText; }
			set { SetProperty(ref m_RichText, value); }
		}

		private ObservableCollection<UserPresenceDataEntry> m_DataEntries = new ObservableCollection<UserPresenceDataEntry>();
		public ObservableCollection<UserPresenceDataEntry> DataEntries
		{
			get { return m_DataEntries; }
			set { SetProperty(ref m_DataEntries, value); }
		}

		private PresenceModification m_CurrentModification;

		private ulong? NotifyOnPresenceChangedId;

		public UserPresenceComponent()
		{
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			if (!CanUsePlatform)
			{
				return;
			}

			if (NotifyOnPresenceChangedId == null)
			{
				var addNotifyOnPresenceChangedOptions = new AddNotifyOnPresenceChangedOptions();
				ulong notifyId = User.PlatformApplication.PlatformInterface.GetPresenceInterface().AddNotifyOnPresenceChanged(ref addNotifyOnPresenceChangedOptions, null, OnPresenceChangedCallback);
				if (notifyId != Common.InvalidNotificationid)
				{
					NotifyOnPresenceChangedId = notifyId;
				}

				//UpdatePresence();
			}
		}

		protected override void OnDisposing()
		{
			base.OnDisposing();

			if (CanUsePlatform)
			{
				return;
			}

			if (NotifyOnPresenceChangedId.HasValue)
			{
				User.PlatformApplication.PlatformInterface.GetPresenceInterface().RemoveNotifyOnPresenceChanged(NotifyOnPresenceChangedId.Value);
				NotifyOnPresenceChangedId = null;
			}
		}

		private void OnPresenceChangedCallback(ref PresenceChangedCallbackInfo PresenceChangedCallbackInfo)
		{
			if (PresenceChangedCallbackInfo.PresenceUserId == User.EpicAccountId)
			{
				UpdatePresence();
			}
		}

		private void UpdatePresence()
		{
			if (UpdateState == UpdateState.InProgress || User.EpicAccountId == null || User.PlatformApplication.PlatformInterface == null)
			{
				return;
			}

			UpdateState = UpdateState.InProgress;

			var queryPresenceOptions = new QueryPresenceOptions()
			{
				LocalUserId = User.PlatformApplication.FirstLocalUser.EpicAccountId,
				TargetUserId = User.EpicAccountId
			};

			User.PlatformApplication.PlatformInterface.GetPresenceInterface().QueryPresence(ref queryPresenceOptions, null, OnQueryPresence);
		}

		private void OnQueryPresence(ref QueryPresenceCallbackInfo queryPresenceCallbackInfo)
		{
			if (queryPresenceCallbackInfo.ResultCode == Result.Success)
			{
				var copyPresenceOptions = new CopyPresenceOptions()
				{
					LocalUserId = queryPresenceCallbackInfo.LocalUserId,
					TargetUserId = queryPresenceCallbackInfo.TargetUserId
				};

				var result = User.PlatformApplication.PlatformInterface.GetPresenceInterface().CopyPresence(ref copyPresenceOptions, out var info);
				if (result == Result.Success)
				{
					Status = info.Value.Status;
					ProductIdText = info.Value.ProductId;
					ProductVersionText = info.Value.ProductVersion;
					PlatformText = info.Value.Platform;
					RichText = info.Value.RichText;

					DataEntries.Clear();

					if (info.Value.Records != null)
					{
						foreach (var record in info.Value.Records.Select(record => new UserPresenceDataEntry() { Key = record.Key, Value = record.Value }))
						{
							DataEntries.Add(record);
						}
					}
				}
			}

			if (Common.IsOperationComplete(queryPresenceCallbackInfo.ResultCode))
			{
				UpdateState = UpdateState.Done;
			}
		}

		public void Modify(UserPresenceModification presenceModificationData)
		{
			if (m_CurrentModification != null)
			{
				return;
			}

			var createPresenceModificationOptions = new CreatePresenceModificationOptions()
			{
				LocalUserId = User.EpicAccountId
			};

			var result = User.PlatformApplication.PlatformInterface.GetPresenceInterface().CreatePresenceModification(ref createPresenceModificationOptions, out m_CurrentModification);
			Log.WriteResult("CreatePresenceModification", result);
			if (result == Result.Success)
			{
				if (presenceModificationData.Status.HasValue)
				{
					var setStatusOptions = new PresenceModificationSetStatusOptions()
					{
						Status = presenceModificationData.Status.Value
					};

					result = m_CurrentModification.SetStatus(ref setStatusOptions);
					Log.WriteResult($"SetStatus", result);
				}

				if (presenceModificationData.RichText != null)
				{
					var setRawRichTextOptions = new PresenceModificationSetRawRichTextOptions()
					{
						RichText = presenceModificationData.RichText
					};

					result = m_CurrentModification.SetRawRichText(ref setRawRichTextOptions);
					Log.WriteResult("SetRawRichText", result);
				}

				if (presenceModificationData.DataEntries != null)
				{
					var setDataOptions = new PresenceModificationSetDataOptions()
					{
						Records = presenceModificationData.DataEntries.Select(dataEntry => new DataRecord { Key = dataEntry.Key, Value = dataEntry.Value }).ToArray()
					};

					result = m_CurrentModification.SetData(ref setDataOptions);
					Log.WriteResult("SetData", result);

					var keysAdded = presenceModificationData.DataEntries.Select(dataEntry => dataEntry.Key);
					var keysToRemove = presenceModificationData.DataEntries.Where(dataEntry => !keysAdded.Contains(dataEntry.Key)).Select(dataEntry => dataEntry.Key);
					if (keysToRemove.Any())
					{
						var deleteDataOptions = new PresenceModificationDeleteDataOptions()
						{
							Records = keysToRemove.Select(key => new PresenceModificationDataRecordId { Key = key }).ToArray()
						};

						result = m_CurrentModification.DeleteData(ref deleteDataOptions);
						Log.WriteResult("DeleteData", result);
					}
				}

				var setPresenceOptions = new SetPresenceOptions()
				{
					LocalUserId = User.EpicAccountId,
					PresenceModificationHandle = m_CurrentModification
				};

				User.PlatformApplication.PlatformInterface.GetPresenceInterface().SetPresence(ref setPresenceOptions, null, OnSetPresence);
			}
		}

		private void OnSetPresence(ref SetPresenceCallbackInfo setPresenceCallbackInfo)
		{
			Log.WriteResult($"OnSetPresence", setPresenceCallbackInfo.ResultCode);
			if (Common.IsOperationComplete(setPresenceCallbackInfo.ResultCode))
			{
				if (m_CurrentModification != null)
				{
					m_CurrentModification.Release();
					m_CurrentModification = null;
				}
			}
		}
	}
}
