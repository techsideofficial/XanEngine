// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "AntiCheatNetworkTransport.h"

#include "eos_anticheatserver_types.h"
#include "eos_types.h"

class FAntiCheatServer
{
public:
	void Init(EOS_HPlatform Platform);
	void BeginSession();
	void EndSession();

	void RegisterClient(void* ClientHandle, FAntiCheatNetworkTransport::FRegistrationInfoMessage Message);
	void UnregisterClient(void* ClientHandle);

	void OnMessageFromClientReceived(void* ClientHandle, const void* Data, uint32_t DataLengthBytes);
	
private:
	static void EOS_CALL OnMessageToClientCb(const EOS_AntiCheatCommon_OnMessageToClientCallbackInfo* Data);
	static void EOS_CALL OnClientActionRequiredCb(const EOS_AntiCheatCommon_OnClientActionRequiredCallbackInfo* Data);

private:
	EOS_HAntiCheatServer AntiCheatServerHandle;

	EOS_NotificationId MessageToClientId;
	EOS_NotificationId ClientActionRequiredId;
};

