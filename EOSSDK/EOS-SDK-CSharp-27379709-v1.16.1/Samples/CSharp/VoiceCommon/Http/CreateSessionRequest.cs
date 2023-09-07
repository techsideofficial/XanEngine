// Copyright Epic Games, Inc. All Rights Reserved.

using Newtonsoft.Json;

namespace Epic.OnlineServices.Http
{
	public class CreateSessionRequest
	{
		[JsonProperty("puid")]
		public string ProductUserId;

		[JsonProperty("password")]
		public string Password;
	}
}
