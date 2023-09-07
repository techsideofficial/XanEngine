// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Achievements
{
	/// <summary>
	/// Function prototype definition for callbacks passed to <see cref="AchievementsInterface.UnlockAchievements" />
	/// </summary>
	/// <param name="data">An <see cref="OnUnlockAchievementsCompleteCallbackInfo" /> containing the output information and result</param>
	public delegate void OnUnlockAchievementsCompleteCallback(ref OnUnlockAchievementsCompleteCallbackInfo data);

	[System.Runtime.InteropServices.UnmanagedFunctionPointer(Config.LibraryCallingConvention)]
	internal delegate void OnUnlockAchievementsCompleteCallbackInternal(ref OnUnlockAchievementsCompleteCallbackInfoInternal data);
}