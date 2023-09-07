// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.AntiCheatClient
{
	/// <summary>
	/// Callback issued when an action must be applied to a connected peer.
	/// This callback is always issued from within <see cref="Platform.PlatformInterface.Tick" /> on its calling thread.
	/// </summary>
	public delegate void OnPeerActionRequiredCallback(ref AntiCheatCommon.OnClientActionRequiredCallbackInfo data);

	[System.Runtime.InteropServices.UnmanagedFunctionPointer(Config.LibraryCallingConvention)]
	internal delegate void OnPeerActionRequiredCallbackInternal(ref AntiCheatCommon.OnClientActionRequiredCallbackInfoInternal data);
}