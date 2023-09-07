// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Ecom
{
	/// <summary>
	/// Input parameters for the <see cref="EcomInterface.CopyEntitlementByNameAndIndex" /> function.
	/// </summary>
	public struct CopyEntitlementByNameAndIndexOptions
	{
		/// <summary>
		/// The Epic Account ID of the local user whose entitlement is being copied
		/// </summary>
		public EpicAccountId LocalUserId { get; set; }

		/// <summary>
		/// Name of the entitlement to retrieve from the cache
		/// </summary>
		public Utf8String EntitlementName { get; set; }

		/// <summary>
		/// Index of the entitlement within the named set to retrieve from the cache.
		/// </summary>
		public uint Index { get; set; }
	}

	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 8)]
	internal struct CopyEntitlementByNameAndIndexOptionsInternal : ISettable<CopyEntitlementByNameAndIndexOptions>, System.IDisposable
	{
		private int m_ApiVersion;
		private System.IntPtr m_LocalUserId;
		private System.IntPtr m_EntitlementName;
		private uint m_Index;

		public EpicAccountId LocalUserId
		{
			set
			{
				Helper.Set(value, ref m_LocalUserId);
			}
		}

		public Utf8String EntitlementName
		{
			set
			{
				Helper.Set(value, ref m_EntitlementName);
			}
		}

		public uint Index
		{
			set
			{
				m_Index = value;
			}
		}

		public void Set(ref CopyEntitlementByNameAndIndexOptions other)
		{
			m_ApiVersion = EcomInterface.CopyentitlementbynameandindexApiLatest;
			LocalUserId = other.LocalUserId;
			EntitlementName = other.EntitlementName;
			Index = other.Index;
		}

		public void Set(ref CopyEntitlementByNameAndIndexOptions? other)
		{
			if (other.HasValue)
			{
				m_ApiVersion = EcomInterface.CopyentitlementbynameandindexApiLatest;
				LocalUserId = other.Value.LocalUserId;
				EntitlementName = other.Value.EntitlementName;
				Index = other.Value.Index;
			}
		}

		public void Dispose()
		{
			Helper.Dispose(ref m_LocalUserId);
			Helper.Dispose(ref m_EntitlementName);
		}
	}
}