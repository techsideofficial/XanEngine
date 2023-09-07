// Copyright Epic Games, Inc. All Rights Reserved.

using Newtonsoft.Json;

namespace Epic.OnlineServices.Http
{
	public class HeartbeatRequest
	{
		[JsonProperty("lock")]
		public string OwnerLock;
	}
}
