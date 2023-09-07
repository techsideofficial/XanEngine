// Copyright Epic Games, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;

namespace Epic.OnlineServices.Samples
{
	public class VoiceRoom
	{
		public string Name { get; private set; }

		public string OwnerLock { get; private set; }

		public string ClientBaseUrl { get; private set; }

		public DateTimeOffset LastHeartbeatTime { get; private set; } = DateTimeOffset.UtcNow;

		private List<ProductUserId> m_BannedProductUserIds = new List<ProductUserId>();

		private object m_Lock = new object();

		public VoiceRoom(string name, string ownerLock, string clientBaseUrl)
		{
			Name = name;
			OwnerLock = ownerLock;
			ClientBaseUrl = clientBaseUrl;
		}

		public void Ban(ProductUserId productUserId)
		{
			lock (m_Lock)
			{
				m_BannedProductUserIds.Add(productUserId);
			}
		}

		public bool IsBanned(ProductUserId productUserId)
		{
			lock (m_Lock)
			{
				return m_BannedProductUserIds.Contains(productUserId);
			}
		}

		public void Heartbeat()
		{
			LastHeartbeatTime = DateTimeOffset.UtcNow;
		}
	}
}
