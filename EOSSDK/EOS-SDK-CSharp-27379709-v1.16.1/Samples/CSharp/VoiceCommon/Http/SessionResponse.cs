// Copyright Epic Games, Inc. All Rights Reserved.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Epic.OnlineServices.Http
{
	public class SessionResponse
	{
		[JsonProperty("sessionId")]
		public string RoomName;

		[JsonProperty("ownerLock")]
		public string OwnerLock;

		[JsonProperty("clientBaseUrl")]
		public string ClientBaseUrl;

		[JsonProperty("joinTokens")]
		public Dictionary<string, string> JoinTokens;
	}
}
