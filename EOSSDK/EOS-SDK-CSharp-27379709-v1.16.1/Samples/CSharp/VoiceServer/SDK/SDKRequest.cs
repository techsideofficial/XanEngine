// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.SDK
{
	public abstract class SDKRequest
	{
		public bool IsStarted { get; private set; }

		public abstract bool IsCompleted { get; }

		/// <summary>
		/// Starts the request. Must be called from the main thread.
		/// </summary>
		public virtual void Start()
		{
			if (IsStarted)
			{
				return;
			}

			IsStarted = true;
			StartOperation();
		}

		public abstract void StartOperation();
	}
}
