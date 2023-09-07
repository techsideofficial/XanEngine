// Copyright Epic Games, Inc. All Rights Reserved.

using Newtonsoft.Json;

namespace Epic.OnlineServices.Http
{
	public class ErrorResponse
	{
		[JsonProperty("error")]
		public string Name;

		[JsonProperty("description")]
		public string Description;
	}
}
