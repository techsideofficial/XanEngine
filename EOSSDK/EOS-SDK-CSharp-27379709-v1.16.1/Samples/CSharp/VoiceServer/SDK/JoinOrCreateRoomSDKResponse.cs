// Copyright Epic Games, Inc. All Rights Reserved.

using System.Collections.Generic;

namespace Epic.OnlineServices.Samples.SDK
{
	public class JoinOrCreateRoomSDKResponse : SDKResponse
	{
		public string RoomName { get; set; }

		public string ClientBaseUrl { get; set; }

		public Dictionary<ProductUserId, string> ProductUserIdTokens { get; set; } = new Dictionary<ProductUserId, string>();
	}
}