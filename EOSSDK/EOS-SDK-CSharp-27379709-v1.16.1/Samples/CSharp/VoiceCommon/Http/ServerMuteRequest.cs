// Copyright Epic Games, Inc. All Rights Reserved.

using Newtonsoft.Json;

namespace Epic.OnlineServices.Http
{
	public class ServerMuteRequest
	{
		[JsonProperty("lock")]
		public string OwnerLock;

		[JsonProperty("mute")]
		public bool IsMuted;
	}
}
