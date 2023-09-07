// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Platform;
using Epic.OnlineServices.Samples.ViewModels.Menus;
using Epic.OnlineServices.Samples.ViewModels.UserComponents;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace Epic.OnlineServices.Samples.ViewModels
{
	public class PlatformApplication : Bindable
	{
		public PlatformInterface PlatformInterface { get; set; }

		public ObservableCollection<LogMessage> LogMessages { get; set; } = new ObservableCollection<LogMessage>();

		private ObservableCollection<User> m_Users = new ObservableCollection<User>();
		public ObservableCollection<User> Users
		{
			get { return m_Users; }
			set
			{
				if (SetProperty(ref m_Users, value) && value != null)
				{
					foreach (var user in value)
					{
						user.PlatformApplication = this;
					}
				}
			}
		}

		private UserAuthMenu m_UserAuthMenu;
		public UserAuthMenu UserAuthMenu
		{
			get { return m_UserAuthMenu; }
			set
			{
				if (SetProperty(ref m_UserAuthMenu, value) && value != null)
				{
					value.PlatformApplication = this;
				}
			}
		}

		private UserMenu m_UserFeatureMenu;
		public UserMenu UserFeatureMenu
		{
			get { return m_UserFeatureMenu; }
			set
			{
				if (SetProperty(ref m_UserFeatureMenu, value) && value != null)
				{
					value.PlatformApplication = this;
				}
			}
		}

		public User FirstLocalUser
		{
			get { return Users.FirstOrDefault(user => user.IsLocalUser); }
		}

		protected bool IsDisposed { get; private set; }

		protected bool CanUsePlatform
		{
			get
			{
				return !(IsDisposed || PlatformInterface == null);
			}
		}

		private Application m_Application;
		private Window m_Window;

		private DispatcherTimer m_UpdateTimer;
		private const float c_UpdateFrequency = 1 / 30f;
		private Timer m_TickTimer;

		public PlatformApplication()
		{
		}

		public int Run<TWindow, TUserFeatureMenu>(bool requiresConnect)
			where TWindow : Window, new()
			where TUserFeatureMenu : UserMenu, new()
		{
			Settings.Initialize();

			UserAuthMenu = new UserAuthMenu(requiresConnect);
			UserAuthMenu.SetUser(new User(this));

			UserFeatureMenu = new TUserFeatureMenu();
			UserFeatureMenu.SetUser(null);

			Log.LineReceived += (logLine) => LogMessages.Add(new LogMessage() { Text = logLine.Message, Style = logLine.Style });

			m_Window = new TWindow();
			m_Window.DataContext = this;
			m_Window.Show();

			PlatformInterface = EpicApplication.Initialize(PlatformFlags.DisableOverlay);
			if (PlatformInterface != null)
			{
				m_TickTimer = new Timer()
				{
					MaxTime = 0.1f,
					Complete = () => PlatformInterface?.Tick()
				};

				m_UpdateTimer = new DispatcherTimer(DispatcherPriority.Render)
				{
					Interval = new TimeSpan(0, 0, 0, 0, (int)(c_UpdateFrequency * 1000))
				};

				m_UpdateTimer.Tick += (sender, e) => Update(c_UpdateFrequency);
				m_UpdateTimer.Start();

				if (Settings.IsAutoLoginEnabled)
				{
					UserAuthMenu.AutoLogin();
				}
			}

			m_Application = new Application();
			m_Application.Exit += (sender, e) => Dispose();
			return m_Application.Run();
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

		protected void OnDisposing()
		{
			if (!CanUsePlatform)
			{
				return;
			}

			if (Users != null)
			{
				foreach (var user in Users)
				{
					user.Dispose();
				}

				Users.Clear();
			}

			PlatformInterface.Release();
			PlatformInterface = null;

			PlatformInterface.Shutdown();
		}

		public void AddUser(User user)
		{
			if (!Users.Contains(user))
			{
				user.PlatformApplication = this;
				var originalFirstLocalUser = FirstLocalUser;

				user.GetOrAddComponent<UserPresenceComponent>();
				user.GetOrAddComponent<UserInfoComponent>();

				Users.Add(user);
				NotifyPropertyChanged(nameof(FirstLocalUser));

				// Set the active user on the menus
				if (originalFirstLocalUser == null && FirstLocalUser != null)
				{
					UserAuthMenu.SetUser(null);
					UserFeatureMenu.SetUser(FirstLocalUser);
				}
			}
		}

		public void RemoveUser(User user)
		{
			if (Users.Contains(user))
			{
				var originalFirstLocalUser = FirstLocalUser;

				m_Users.Remove(user);
				NotifyPropertyChanged(nameof(FirstLocalUser));

				// Set the active user on the menus
				if (originalFirstLocalUser != null && FirstLocalUser == null)
				{
					UserAuthMenu.SetUser(new User(this));
					UserFeatureMenu.SetUser(null);
				}
			}
		}

		private void Update(float deltaTime)
		{
			m_TickTimer.Update(deltaTime);

			foreach (var user in Users)
			{
				user.Update(deltaTime);
			}

			UserAuthMenu.Update(deltaTime);
			UserFeatureMenu.Update(deltaTime);
		}
	}
}
