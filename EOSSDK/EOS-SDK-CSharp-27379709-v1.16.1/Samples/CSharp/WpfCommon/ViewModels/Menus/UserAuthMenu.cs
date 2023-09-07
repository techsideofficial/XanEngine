// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Samples.ViewModels.UserComponents;
using System.Diagnostics;

namespace Epic.OnlineServices.Samples.ViewModels.Menus
{
	public class UserAuthMenu : UserComponentMenu<UserAuthComponent>
	{
		private LoginCredentialType m_CredentialType;
		public LoginCredentialType CredentialType
		{
			get { return m_CredentialType; }
			set { SetProperty(ref m_CredentialType, value); }
		}

		private ExternalCredentialType m_ExternalCredentialType;
		public ExternalCredentialType ExternalCredentialType
		{
			get { return m_ExternalCredentialType; }
			set { SetProperty(ref m_ExternalCredentialType, value); }
		}
		
		private string m_Id;
		public string Id
		{
			get { return m_Id; }
			set { SetProperty(ref m_Id, value); }
		}

		private string m_Token;
		public string Token
		{
			get { return m_Token; }
			set { SetProperty(ref m_Token, value); }
		}

		public DelegateCommand LoginCommand { get; private set; }

		private bool m_RequiresConnect;

		public UserAuthMenu()
		{
		}

		public UserAuthMenu(bool requiresConnect)
		{
			m_RequiresConnect = requiresConnect;

			CredentialType = Settings.LoginCredentialType;
			ExternalCredentialType = Settings.ExternalCredentialType;
			Id = Settings.Id;
			Token = Settings.Token;

			LoginCommand = new DelegateCommand((parameter) =>
			{
				Login();
			});
		}

		protected override void OnUserChanged(User oldUser, User newUser)
		{
			base.OnUserChanged(oldUser, newUser);

			if (oldUser != null)
			{
				oldUser.GetOrAddComponent<UserAuthComponent>().LoginStateChanged -= UserAuthComponent_LoginStateChanged;
			}

			if (newUser != null)
			{
				newUser.GetOrAddComponent<UserAuthComponent>().LoginStateChanged += UserAuthComponent_LoginStateChanged;
			}
		}

		private void UserAuthComponent_LoginStateChanged(UserAuthComponent userAuthComponent, UserAuthLoginState fromLoginState, UserAuthLoginState toLoginState)
		{
			if (toLoginState == UserAuthLoginState.LoggedIn)
			{
				PlatformApplication.AddUser(userAuthComponent.User);
			}
		}

		public void AutoLogin()
		{
			// If either of these fields are provided, attempt normal login with the provided login type
			if (!string.IsNullOrEmpty(Id) || !string.IsNullOrEmpty(Token))
			{
				Login();
			}

			// Otherwise, fallback onto persistent login
			else
			{
				UserComponent.PersistentLogin();
			}
		}

		private void Login()
		{
			var credentials = new Credentials()
			{
				Type = CredentialType,
				Id = Id,
				Token = Token,
				ExternalType = ExternalCredentialType
			};

			UserComponent.Login(credentials, m_RequiresConnect);
		}
	}
}