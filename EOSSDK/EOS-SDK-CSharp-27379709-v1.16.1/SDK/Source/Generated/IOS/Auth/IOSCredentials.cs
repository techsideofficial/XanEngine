// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Auth
{
	/// <summary>
	/// Login credentials filled as part of the <see cref="LoginOptions" /> struct for <see cref="AuthInterface.Login" /> API.
	/// 
	/// Required input parameters to be set depend on the login credential type.
	/// Any parameters not being used must be set to <see langword="null" />. Otherwise, <see cref="Result.InvalidParameters" /> error is returned.
	/// 
	/// <see cref="LoginCredentialType.Password" /> | ID is the email address, and Token is the password.
	/// <see cref="LoginCredentialType.ExchangeCode" /> | Set ID to <see langword="null" />. Token is the exchange code.
	/// <see cref="LoginCredentialType.PersistentAuth" /> | Set ID to <see langword="null" />. On console platforms, Token is the long-lived refresh token. Otherwise, set to <see langword="null" />.
	/// <see cref="LoginCredentialType.Developer" /> | Set ID as the host (e.g. localhost:6547). Token is the credential name registered in the EOS Developer Authentication Tool.
	/// <see cref="LoginCredentialType.RefreshToken" /> | Set ID to <see langword="null" />. Token is the refresh token.
	/// <see cref="LoginCredentialType.AccountPortal" /> | Set ID and Token to <see langword="null" />. SystemAuthCredentialsOptions may be required on mobile platforms.
	/// <see cref="LoginCredentialType.ExternalAuth" /> | Set ID to <see langword="null" /> or the External Account ID that belongs to the external auth token. Token is the external auth token specified by ExternalType. External Account IDs set to the ID are expected as either base-10 numeric strings for integer-based external Account IDs, or the actual string for everything else. If ID is provided, login will automatically be cancelled if the EOS SDK is able to and does detect the external account signing-out. If ID is provided, it must match the external account ID belonging to the auth-token, or login will fail.
	/// <seealso cref="LoginCredentialType" />
	/// <seealso cref="AuthInterface.Login" />
	/// <seealso cref="DeletePersistentAuthOptions" />
	/// </summary>
	public struct IOSCredentials
	{
		/// <summary>
		/// Authentication ID value based on the used <see cref="LoginCredentialType" />.
		/// If not used, must be set to <see langword="null" />.
		/// </summary>
		public Utf8String Id { get; set; }

		/// <summary>
		/// Authentication Token value based on the used <see cref="LoginCredentialType" />.
		/// If not used, must be set to <see langword="null" />.
		/// </summary>
		public Utf8String Token { get; set; }

		/// <summary>
		/// Login credentials type based on the authentication method used.
		/// </summary>
		public LoginCredentialType Type { get; set; }

		/// <summary>
		/// This field is for system specific options, if any.
		/// 
		/// If provided, the structure will be located in (System)/eos_(system).h.
		/// The structure will be named EOS_(System)_Auth_CredentialsOptions.
		/// </summary>
		public IOSCredentialsSystemAuthCredentialsOptions? SystemAuthCredentialsOptions { get; set; }

		/// <summary>
		/// Type of external login. Needed to identify the external auth method to use.
		/// Used when login type is set to <see cref="LoginCredentialType.ExternalAuth" />, ignored for other <see cref="LoginCredentialType" /> methods.
		/// </summary>
		public ExternalCredentialType ExternalType { get; set; }

		internal void Set(ref IOSCredentialsInternal other)
		{
			Id = other.Id;
			Token = other.Token;
			Type = other.Type;
			SystemAuthCredentialsOptions = other.SystemAuthCredentialsOptions;
			ExternalType = other.ExternalType;
		}
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct IOSCredentialsInternal : IGettable<IOSCredentials>, ISettable<IOSCredentials>, System.IDisposable
	{
		private int m_ApiVersion;
		private System.IntPtr m_Id;
		private System.IntPtr m_Token;
		private LoginCredentialType m_Type;
		private System.IntPtr m_SystemAuthCredentialsOptions;
		private ExternalCredentialType m_ExternalType;

		public Utf8String Id
		{
			get
			{
				Utf8String value;
				Helper.Get(m_Id, out value);
				return value;
			}

			set
			{
				Helper.Set(value, ref m_Id);
			}
		}

		public Utf8String Token
		{
			get
			{
				Utf8String value;
				Helper.Get(m_Token, out value);
				return value;
			}

			set
			{
				Helper.Set(value, ref m_Token);
			}
		}

		public LoginCredentialType Type
		{
			get
			{
				return m_Type;
			}

			set
			{
				m_Type = value;
			}
		}

		public IOSCredentialsSystemAuthCredentialsOptions? SystemAuthCredentialsOptions
		{
			get
			{
				IOSCredentialsSystemAuthCredentialsOptions? value;
				Helper.Get<IOSCredentialsSystemAuthCredentialsOptionsInternal, IOSCredentialsSystemAuthCredentialsOptions>(m_SystemAuthCredentialsOptions, out value);
				return value;
			}

			set
			{
				Helper.Set<IOSCredentialsSystemAuthCredentialsOptions, IOSCredentialsSystemAuthCredentialsOptionsInternal>(ref value, ref m_SystemAuthCredentialsOptions);
			}
		}

		public ExternalCredentialType ExternalType
		{
			get
			{
				return m_ExternalType;
			}

			set
			{
				m_ExternalType = value;
			}
		}

		public void Set(ref IOSCredentials other)
		{
			m_ApiVersion = AuthInterface.CredentialsApiLatest;
			Id = other.Id;
			Token = other.Token;
			Type = other.Type;
			SystemAuthCredentialsOptions = other.SystemAuthCredentialsOptions;
			ExternalType = other.ExternalType;
		}

		public void Set(ref IOSCredentials? other)
		{
			if (other.HasValue)
			{
				m_ApiVersion = AuthInterface.CredentialsApiLatest;
				Id = other.Value.Id;
				Token = other.Value.Token;
				Type = other.Value.Type;
				SystemAuthCredentialsOptions = other.Value.SystemAuthCredentialsOptions;
				ExternalType = other.Value.ExternalType;
			}
		}

		public void Dispose()
		{
			Helper.Dispose(ref m_Id);
			Helper.Dispose(ref m_Token);
			Helper.Dispose(ref m_SystemAuthCredentialsOptions);
		}

		public void Get(out IOSCredentials output)
		{
			output = new IOSCredentials();
			output.Set(ref this);
		}
	}
}