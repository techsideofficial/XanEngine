// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.ViewModels.Menus
{
	public abstract class UserMenu : Bindable
	{
		public PlatformApplication PlatformApplication { get; set; }

		private User m_User;
		public User User
		{
			get { return m_User; }
			set { SetProperty(ref m_User, value); }
		}

		public UserMenu()
		{
		}

		public virtual void Update(float deltaTime)
		{
		}

		public void SetUser(User newUser)
		{
			User oldUser = m_User;
			if (oldUser != newUser)
			{
				User = newUser;
				OnUserChanged(oldUser, newUser);
			}
		}

		protected virtual void OnUserChanged(User oldUser, User newUser)
		{
		}
	}
}
