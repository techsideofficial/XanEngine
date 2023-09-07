// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.ViewModels.UserComponents
{
	public abstract class UserComponent : Bindable
	{
		public User User { get; set; }

		protected bool IsDisposed { get; private set; }

		protected bool CanUsePlatform
		{
			get
			{
				return !(IsDisposed || User == null || User.PlatformApplication == null || User.PlatformApplication.PlatformInterface == null);
			}
		}

		public UserComponent()
		{
		}

		public virtual void Update(float deltaTime)
		{
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
		}
	}
}
