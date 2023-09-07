// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Samples.ViewModels.UserComponents;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Epic.OnlineServices.Samples.ViewModels
{
	public class User : Bindable
	{
		public PlatformApplication PlatformApplication { get; set; }

		public EpicAccountId EpicAccountId { get; set; }

		public ProductUserId ProductUserId { get; set; }

		private ObservableCollection<UserComponent> m_Components = new ObservableCollection<UserComponent>();
		public ObservableCollection<UserComponent> Components
		{
			get { return m_Components; }
			set
			{
				if (SetProperty(ref m_Components, value) && value != null)
				{
					foreach (var component in value)
					{
						component.User = this;
					}
				}
			}
		}

		public bool IsLocalUser
		{
			get { return HasComponent<UserAuthComponent>(); }
		}

		protected bool IsDisposed { get; private set; }

		protected bool CanUsePlatform
		{
			get
			{
				return !(IsDisposed || PlatformApplication == null || PlatformApplication.PlatformInterface == null);
			}
		}

		public User()
		{
		}

		public User(PlatformApplication platformApplication)
		{
			PlatformApplication = platformApplication;
		}

		public User(PlatformApplication platformApplication, ProductUserId productUserId)
		{
			PlatformApplication = platformApplication;

			ProductUserId = productUserId;
			UpdateEpicAccountIdFromProductUserId();
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			OnDisposing();
			IsDisposed = true;
		}

		protected virtual void OnDisposing()
		{
			if (Components == null)
			{
				return;
			}

			foreach (var component in Components)
			{
				component.Dispose();
			}

			Components.Clear();
		}

		public void Update(float deltaTime)
		{
			if (Components == null)
			{
				return;
			}

			foreach (var component in Components)
			{
				component.Update(deltaTime);
			}
		}

		public bool HasComponent<T>()
			where T : UserComponent
		{
			return Components.Where(component => component is T).Any();
		}

		public UserComponent GetComponent(Type type)
		{
			return Components.SingleOrDefault(component => component.GetType() == type);
		}

		public TUserComponent GetOrAddComponent<TUserComponent>()
			where TUserComponent : UserComponent, new()
		{
			var existingComponent = Components.SingleOrDefault(component => component is TUserComponent) as TUserComponent;
			if (existingComponent == null)
			{
				existingComponent = new TUserComponent()
				{
					User = this
				};

				m_Components.Add(existingComponent);

				NotifyPropertyChanged(nameof(IsLocalUser));
			}

			return existingComponent;
		}

		public void SetEpicAccountId(EpicAccountId epicAccountId)
		{
			EpicAccountId = epicAccountId;
		}

		public void UpdateEpicAccountIdFromProductUserId()
		{
			if (!CanUsePlatform || ProductUserId == null || EpicAccountId != null)
			{
				return;
			}

			var queryProductUserIdMappingsOptions = new Connect.QueryProductUserIdMappingsOptions()
			{
				LocalUserId = PlatformApplication.FirstLocalUser.ProductUserId,
				ProductUserIds = new ProductUserId[] { ProductUserId }
			};

			PlatformApplication.PlatformInterface.GetConnectInterface().QueryProductUserIdMappings(ref queryProductUserIdMappingsOptions, null, OnQueryProductUserIdMappings);
		}

		private void OnQueryProductUserIdMappings(ref Connect.QueryProductUserIdMappingsCallbackInfo queryProductUserIdMappingsCallbackInfo)
		{
			Log.WriteResult($"OnQueryProductUserIdMappings", queryProductUserIdMappingsCallbackInfo.ResultCode);
			if (queryProductUserIdMappingsCallbackInfo.ResultCode == Result.Success)
			{
				var getProductUserIdMappingOptions = new Connect.GetProductUserIdMappingOptions()
				{
					LocalUserId = PlatformApplication.FirstLocalUser.ProductUserId,
					TargetProductUserId = ProductUserId
				};

				Utf8String epicAccountIdString;
				var result = PlatformApplication.PlatformInterface.GetConnectInterface().GetProductUserIdMapping(ref getProductUserIdMappingOptions, out epicAccountIdString);
				Log.WriteResult($"GetProductUserIdMapping", result);
				if (result == Result.Success)
				{
					EpicAccountId = EpicAccountId.FromString(epicAccountIdString);
				}
			}
		}
	}
}
