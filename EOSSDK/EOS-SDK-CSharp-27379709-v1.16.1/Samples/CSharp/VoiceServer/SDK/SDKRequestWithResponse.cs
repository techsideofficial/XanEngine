// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.SDK
{
	public abstract class SDKRequestWithResponse<TSDKResponse> : SDKRequest
		where TSDKResponse : SDKResponse
	{
		public TSDKResponse Response { get; private set; }

		public override bool IsCompleted
		{
			get { return Response != null; }
		}

		protected void Complete(TSDKResponse response)
		{
			Response = response;
		}
	}
}
