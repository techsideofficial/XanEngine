// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Presence;

namespace Epic.OnlineServices.Samples.ViewModels.UserComponents
{
	public class UserPresenceModification
	{
		public Status? Status { get; set; }

		public string RichText { get; set; }

		public UserPresenceDataEntry[] DataEntries { get; set; }
	}
}
