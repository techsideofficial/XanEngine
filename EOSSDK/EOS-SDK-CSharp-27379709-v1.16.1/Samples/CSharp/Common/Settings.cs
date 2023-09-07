// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect;
using System;

namespace Epic.OnlineServices.Samples
{
	public static class Settings
	{
		public static string ProductId { get; private set; } = "";
		public static string SandboxId { get; private set; } = "";
		public static string DeploymentId { get; private set; } = "";
		public static string ClientId { get; private set; } = "";
		public static string ClientSecret { get; private set; } = "";

		public static LoginCredentialType LoginCredentialType { get; private set; } = LoginCredentialType.Developer;
		/// These fields correspond to <see cref="Credentials.Id" /> and <see cref="Credentials.Token" />, and their use differs based on the login type.
		/// For more information, see <see cref="Credentials" /> and the Auth Interface documentation.
		public static string Id { get; private set; } = "";
		public static string Token { get; private set; } = "";

		public static ExternalCredentialType ExternalCredentialType { get; private set; } = ExternalCredentialType.Epic;

		public static AuthScopeFlags ScopeFlags
		{
			get
			{
				return AuthScopeFlags.BasicProfile | AuthScopeFlags.FriendsList | AuthScopeFlags.Presence;
			}
		}

		public static string VoiceServerHost { get; private set; } = "127.0.0.1";

		public static int VoiceServerPort { get; private set; } = 1234;

		public static string VoiceServerPassword { get; private set; } = "testpassword";

		public static bool IsAutoLoginEnabled { get; private set; } = false;

		public static void Initialize()
		{
			ProductId = Environment.GetCommandLineArgs().ReadArg("productid", ProductId);
			SandboxId = Environment.GetCommandLineArgs().ReadArg("sandboxid", SandboxId);
			DeploymentId = Environment.GetCommandLineArgs().ReadArg("deploymentid", DeploymentId);
			ClientId = Environment.GetCommandLineArgs().ReadArg("clientid", ClientId);
			ClientSecret = Environment.GetCommandLineArgs().ReadArg("clientsecret", ClientSecret);

			LoginCredentialType = Environment.GetCommandLineArgs().ReadArg("logincredentialtype", LoginCredentialType);
			Id = Environment.GetCommandLineArgs().ReadArg("id", Id);
			Token = Environment.GetCommandLineArgs().ReadArg("token", Token);
			ExternalCredentialType = Environment.GetCommandLineArgs().ReadArg("externalcredentialtype", ExternalCredentialType);

			VoiceServerHost = Environment.GetCommandLineArgs().ReadArg("voiceserverurl", VoiceServerHost);
			VoiceServerPort = Environment.GetCommandLineArgs().ReadArg("voiceserverport", VoiceServerPort);
			VoiceServerPassword = Environment.GetCommandLineArgs().ReadArg("voiceserverpassword", VoiceServerPassword);

			IsAutoLoginEnabled = Environment.GetCommandLineArgs().ReadArg("autologin", IsAutoLoginEnabled);

#if !DEV
			if (LoginCredentialType != LoginCredentialType.Developer &&
				LoginCredentialType != LoginCredentialType.AccountPortal)
			{
				LoginCredentialType = LoginCredentialType.Developer;
			}
#endif
		}
	}
}