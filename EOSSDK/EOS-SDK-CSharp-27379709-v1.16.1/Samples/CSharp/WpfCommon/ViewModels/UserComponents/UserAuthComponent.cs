// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Auth;

namespace Epic.OnlineServices.Samples.ViewModels.UserComponents
{
	public class UserAuthComponent : UserComponent
	{
		private UserAuthLoginState m_LoginState;
		public UserAuthLoginState LoginState
		{
			get { return m_LoginState; }
			set { SetProperty(ref m_LoginState, value); }
		}

		public DelegateCommand LogoutCommand { get; private set; }

		public event UserAuthLoginStateChanged LoginStateChanged;

		private Credentials? m_CurrentCredentials;

		private bool m_RequiresConnect;

		private ulong? m_NotifyLoginStatusChangedId;

		private ulong? m_ConnectNotifyLoginStatusChangedId;

		private ulong? m_ConnectNotifyAuthExpirationId;

		public UserAuthComponent()
		{
			LogoutCommand = new DelegateCommand((parameter) =>
			{
				Logout();
			});
		}

		private void SetLoginState(UserAuthLoginState toLoginState)
		{
			var fromLoginState = LoginState;
			if (toLoginState == fromLoginState)
			{
				return;
			}

			if (toLoginState == UserAuthLoginState.LoggedOut)
			{
				m_CurrentCredentials = null;
			}

			LoginState = toLoginState;
			LoginStateChanged?.Invoke(this, fromLoginState, toLoginState);
		}

		public void PersistentLogin()
		{
			if (!CanUsePlatform || LoginState != UserAuthLoginState.LoggedOut)
			{
				return;
			}

			SetLoginState(UserAuthLoginState.LoggingIn);

			var loginOptions = new LoginOptions()
			{
				Credentials = new Credentials()
				{
					Type = LoginCredentialType.PersistentAuth
				},
				ScopeFlags = Settings.ScopeFlags
			};

			User.PlatformApplication.PlatformInterface.GetAuthInterface().Login(ref loginOptions, null, OnPersistentAuthLogin);
		}

		private void OnPersistentAuthLogin(ref LoginCallbackInfo loginCallbackInfo)
		{
			Log.WriteResult($"OnPersistentAuthLogin", loginCallbackInfo.ResultCode);

			if (loginCallbackInfo.ResultCode != Result.Success && Common.IsOperationComplete(loginCallbackInfo.ResultCode))
			{
				if (loginCallbackInfo.ResultCode == Result.AuthExpired ||
					loginCallbackInfo.ResultCode == Result.InvalidAuth)
				{
					DeletePersistentAuth();
				}
			}

			OnLogin(ref loginCallbackInfo);
		}

		private void DeletePersistentAuth()
		{
			var deletePersistentAuthOptions = new DeletePersistentAuthOptions();
			User.PlatformApplication.PlatformInterface.GetAuthInterface().DeletePersistentAuth(ref deletePersistentAuthOptions, null, OnDeletePersistentAuth);
		}

		private void OnDeletePersistentAuth(ref DeletePersistentAuthCallbackInfo deletePersistentAuthCallbackInfo)
		{
			Log.WriteResult($"OnDeletePersistentAuth", deletePersistentAuthCallbackInfo.ResultCode);
		}

		public void Login(Credentials credentials, bool requiresConnect)
		{
			if (!CanUsePlatform || LoginState != UserAuthLoginState.LoggedOut)
			{
				return;
			}

			SetLoginState(UserAuthLoginState.LoggingIn);

			m_CurrentCredentials = credentials;
			m_RequiresConnect = requiresConnect;

			var loginOptions = new LoginOptions()
			{
				Credentials = credentials,
				ScopeFlags = Settings.ScopeFlags
			};

			User.PlatformApplication.PlatformInterface.GetAuthInterface().Login(ref loginOptions, null, OnLogin);
		}

		private void OnLogin(ref LoginCallbackInfo loginCallbackInfo)
		{
			Log.WriteResult($"OnLogin", loginCallbackInfo.ResultCode);

			if (loginCallbackInfo.ResultCode == Result.Success)
			{
				User.SetEpicAccountId(loginCallbackInfo.LocalUserId);

				if (!m_NotifyLoginStatusChangedId.HasValue)
				{
					var addNotifyLoginStatusChangedOptions = new AddNotifyLoginStatusChangedOptions();
					var notifyId = User.PlatformApplication.PlatformInterface.GetAuthInterface().AddNotifyLoginStatusChanged(ref addNotifyLoginStatusChangedOptions, null, OnLoginStatusChanged);
					if (notifyId != Common.InvalidNotificationid)
					{
						m_NotifyLoginStatusChangedId = notifyId;
					}
				}

				if (m_RequiresConnect)
				{
					ConnectLogin();
				}
				else
				{
					SetLoginState(UserAuthLoginState.LoggedIn);
				}
			}
			else if (Common.IsOperationComplete(loginCallbackInfo.ResultCode))
			{
				SetLoginState(UserAuthLoginState.LoggedOut);
			}
		}

		private void OnLoginStatusChanged(ref LoginStatusChangedCallbackInfo loginStatusChangedCallbackInfo)
		{
			if (User.EpicAccountId == loginStatusChangedCallbackInfo.LocalUserId)
			{
				if (loginStatusChangedCallbackInfo.PrevStatus == LoginStatus.LoggedIn && loginStatusChangedCallbackInfo.CurrentStatus == LoginStatus.NotLoggedIn)
				{
					if (m_NotifyLoginStatusChangedId.HasValue)
					{
						User.PlatformApplication.PlatformInterface.GetAuthInterface().RemoveNotifyLoginStatusChanged(m_NotifyLoginStatusChangedId.Value);
						m_NotifyLoginStatusChangedId = null;
					}

					SetLoginState(UserAuthLoginState.LoggedOut);

					User.PlatformApplication.RemoveUser(User);
				}
			}
		}

		public void Logout()
		{
			if (!CanUsePlatform || LoginState != UserAuthLoginState.LoggedIn)
			{
				return;
			}

			SetLoginState(UserAuthLoginState.LoggingOut);

			var logoutOptions = new LogoutOptions()
			{
				LocalUserId = User.EpicAccountId,
			};

			User.PlatformApplication.PlatformInterface.GetAuthInterface().Logout(ref logoutOptions, null, OnLogout);
		}

		private void OnLogout(ref LogoutCallbackInfo logoutCallbackInfo)
		{
			Log.WriteResult($"OnLogout", logoutCallbackInfo.ResultCode);
			if (Common.IsOperationComplete(logoutCallbackInfo.ResultCode))
			{
				DeletePersistentAuth();
				SetLoginState(UserAuthLoginState.LoggedOut);
			}
		}

		private Token? CopyUserAuthToken()
		{
			Token? token;

			var copyUserAuthTokenOptions = new CopyUserAuthTokenOptions();
			var result = User.PlatformApplication.PlatformInterface.GetAuthInterface().CopyUserAuthToken(ref copyUserAuthTokenOptions, User.EpicAccountId, out token);
			Log.WriteResult($"CopyUserAuthToken", result);
			if (result == Result.Success)
			{
				return token;
			}

			return null;
		}

		private void ConnectLogin()
		{
			var token = CopyUserAuthToken();
			if (token != null)
			{
				var connectLoginOptions = new Connect.LoginOptions()
				{
					Credentials = new Connect.Credentials()
					{
						Type = m_CurrentCredentials.Value.ExternalType,
						Token = token.Value.AccessToken
					}
				};

				User.PlatformApplication.PlatformInterface.GetConnectInterface().Login(ref connectLoginOptions, null, OnConnectLogin);
			}
			else
			{
				SetLoginState(UserAuthLoginState.LoggedOut);
			}
		}

		private void OnConnectLogin(ref Connect.LoginCallbackInfo loginCallbackInfo)
		{
			Log.WriteResult($"OnConnectLogin", loginCallbackInfo.ResultCode);

			if (loginCallbackInfo.ResultCode == Result.Success)
			{
				User.ProductUserId = loginCallbackInfo.LocalUserId;

				if (!m_ConnectNotifyLoginStatusChangedId.HasValue)
				{
					var addNotifyLoginStatusChangedOptions = new Connect.AddNotifyLoginStatusChangedOptions();
					var notifyId = User.PlatformApplication.PlatformInterface.GetConnectInterface().AddNotifyLoginStatusChanged(ref addNotifyLoginStatusChangedOptions, null, OnConnectLoginStatusChanged);
					if (notifyId != Common.InvalidNotificationid)
					{
						m_ConnectNotifyLoginStatusChangedId = notifyId;
					}
				}

				if (!m_ConnectNotifyAuthExpirationId.HasValue)
				{
					var addNotifyAuthExpirationOptions = new Connect.AddNotifyAuthExpirationOptions();
					var notifyId = User.PlatformApplication.PlatformInterface.GetConnectInterface().AddNotifyAuthExpiration(ref addNotifyAuthExpirationOptions, null, OnConnectAuthExpiration);
					if (notifyId != Common.InvalidNotificationid)
					{
						m_ConnectNotifyAuthExpirationId = notifyId;
					}
				}

				SetLoginState(UserAuthLoginState.LoggedIn);
			}
			else if (loginCallbackInfo.ResultCode == Result.InvalidUser)
			{
				CreateConnectUser(loginCallbackInfo.ContinuanceToken);
			}
			else if (Common.IsOperationComplete(loginCallbackInfo.ResultCode))
			{
				SetLoginState(UserAuthLoginState.LoggedOut);
			}
		}

		private void OnConnectAuthExpiration(ref Connect.AuthExpirationCallbackInfo authExpirationCallbackInfo)
		{
			ConnectLogin();
		}

		private void OnConnectLoginStatusChanged(ref Connect.LoginStatusChangedCallbackInfo loginStatusChangedCallbackInfo)
		{
			if (User.ProductUserId == loginStatusChangedCallbackInfo.LocalUserId)
			{
				if (loginStatusChangedCallbackInfo.PreviousStatus == LoginStatus.LoggedIn && loginStatusChangedCallbackInfo.CurrentStatus == LoginStatus.NotLoggedIn)
				{
					if (m_ConnectNotifyLoginStatusChangedId.HasValue)
					{
						User.PlatformApplication.PlatformInterface.GetAuthInterface().RemoveNotifyLoginStatusChanged(m_ConnectNotifyLoginStatusChangedId.Value);
						m_ConnectNotifyLoginStatusChangedId = null;
					}

					if (m_ConnectNotifyAuthExpirationId.HasValue)
					{
						User.PlatformApplication.PlatformInterface.GetAuthInterface().RemoveNotifyLoginStatusChanged(m_ConnectNotifyAuthExpirationId.Value);
						m_ConnectNotifyAuthExpirationId = null;
					}

					SetLoginState(UserAuthLoginState.LoggedOut);
				}
			}
		}

		private void CreateConnectUser(ContinuanceToken continuanceToken)
		{
			var createUserOptions = new Connect.CreateUserOptions()
			{
				ContinuanceToken = continuanceToken
			};

			User.PlatformApplication.PlatformInterface.GetConnectInterface().CreateUser(ref createUserOptions, null, OnCreateUser);
		}

		private void OnCreateUser(ref Connect.CreateUserCallbackInfo createUserCallbackInfo)
		{
			Log.WriteResult($"OnCreateUser", createUserCallbackInfo.ResultCode);

			if (createUserCallbackInfo.ResultCode == Result.Success)
			{
				ConnectLogin();
			}
			else if (Common.IsOperationComplete(createUserCallbackInfo.ResultCode))
			{
				SetLoginState(UserAuthLoginState.LoggedOut);
			}
		}
	}
}
