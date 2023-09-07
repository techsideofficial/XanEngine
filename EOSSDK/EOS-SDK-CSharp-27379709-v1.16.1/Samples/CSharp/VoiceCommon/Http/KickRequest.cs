// Copyright Epic Games, Inc. All Rights Reserved.

using Newtonsoft.Json;

namespace Epic.OnlineServices.Http
{
	public class KickRequest
	{
		[JsonProperty("lock")]
		public string OwnerLock;
	}
}
