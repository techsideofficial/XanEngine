// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Friends
{
	public sealed partial class FriendsInterface : Handle
	{
		public FriendsInterface()
		{
		}

		public FriendsInterface(System.IntPtr innerHandle) : base(innerHandle)
		{
		}

		/// <summary>
		/// The most recent version of the <see cref="AcceptInvite" /> API.
		/// </summary>
		public const int AcceptinviteApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="AddNotifyBlockedUsersUpdate" /> API.
		/// </summary>
		public const int AddnotifyblockedusersupdateApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="AddNotifyFriendsUpdate" /> API.
		/// </summary>
		public const int AddnotifyfriendsupdateApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="GetBlockedUserAtIndex" /> API.
		/// </summary>
		public const int GetblockeduseratindexApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="GetBlockedUsersCount" /> API.
		/// </summary>
		public const int GetblockeduserscountApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="GetFriendAtIndex" /> API.
		/// </summary>
		public const int GetfriendatindexApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="GetFriendsCount" /> API.
		/// </summary>
		public const int GetfriendscountApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="GetStatus" /> API.
		/// </summary>
		public const int GetstatusApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="QueryFriends" /> API.
		/// </summary>
		public const int QueryfriendsApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="RejectInvite" /> API.
		/// </summary>
		public const int RejectinviteApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="SendInvite" /> API.
		/// </summary>
		public const int SendinviteApiLatest = 1;

		/// <summary>
		/// Starts an asynchronous task that accepts a friend invitation from another user. The completion delegate is executed after the backend response has been received.
		/// </summary>
		/// <param name="options">structure containing the logged in account and the inviting account</param>
		/// <param name="clientData">arbitrary data that is passed back to you in the CompletionDelegate</param>
		/// <param name="completionDelegate">a callback that is fired when the async operation completes, either successfully or in error</param>
		public void AcceptInvite(ref AcceptInviteOptions options, object clientData, OnAcceptInviteCallback completionDelegate)
		{
			AcceptInviteOptionsInternal optionsInternal = new AcceptInviteOptionsInternal();
			optionsInternal.Set(ref options);

			var clientDataAddress = System.IntPtr.Zero;

			var completionDelegateInternal = new OnAcceptInviteCallbackInternal(OnAcceptInviteCallbackInternalImplementation);
			Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);

			Bindings.EOS_Friends_AcceptInvite(InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);

			Helper.Dispose(ref optionsInternal);
		}

		/// <summary>
		/// Listen for changes to blocklist for a particular account.
		/// </summary>
		/// <param name="options">Information about the API version which is being used.</param>
		/// <param name="clientData">This value is returned to the caller when BlockedUsersUpdateHandler is invoked.</param>
		/// <param name="blockedUsersUpdateHandler">The callback to be invoked when a blocklist changes.</param>
		/// <returns>
		/// A valid notification ID if successfully bound, or <see cref="Common.InvalidNotificationid" /> otherwise.
		/// </returns>
		public ulong AddNotifyBlockedUsersUpdate(ref AddNotifyBlockedUsersUpdateOptions options, object clientData, OnBlockedUsersUpdateCallback blockedUsersUpdateHandler)
		{
			AddNotifyBlockedUsersUpdateOptionsInternal optionsInternal = new AddNotifyBlockedUsersUpdateOptionsInternal();
			optionsInternal.Set(ref options);

			var clientDataAddress = System.IntPtr.Zero;

			var blockedUsersUpdateHandlerInternal = new OnBlockedUsersUpdateCallbackInternal(OnBlockedUsersUpdateCallbackInternalImplementation);
			Helper.AddCallback(out clientDataAddress, clientData, blockedUsersUpdateHandler, blockedUsersUpdateHandlerInternal);

			var funcResult = Bindings.EOS_Friends_AddNotifyBlockedUsersUpdate(InnerHandle, ref optionsInternal, clientDataAddress, blockedUsersUpdateHandlerInternal);

			Helper.Dispose(ref optionsInternal);

			Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);

			return funcResult;
		}

		/// <summary>
		/// Listen for changes to friends for a particular account.
		/// </summary>
		/// <param name="options">Information about who would like notifications.</param>
		/// <param name="clientData">This value is returned to the caller when FriendsUpdateHandler is invoked.</param>
		/// <param name="friendsUpdateHandler">The callback to be invoked when a change to any friend status changes.</param>
		/// <returns>
		/// A valid notification ID if successfully bound, or <see cref="Common.InvalidNotificationid" /> otherwise
		/// </returns>
		public ulong AddNotifyFriendsUpdate(ref AddNotifyFriendsUpdateOptions options, object clientData, OnFriendsUpdateCallback friendsUpdateHandler)
		{
			AddNotifyFriendsUpdateOptionsInternal optionsInternal = new AddNotifyFriendsUpdateOptionsInternal();
			optionsInternal.Set(ref options);

			var clientDataAddress = System.IntPtr.Zero;

			var friendsUpdateHandlerInternal = new OnFriendsUpdateCallbackInternal(OnFriendsUpdateCallbackInternalImplementation);
			Helper.AddCallback(out clientDataAddress, clientData, friendsUpdateHandler, friendsUpdateHandlerInternal);

			var funcResult = Bindings.EOS_Friends_AddNotifyFriendsUpdate(InnerHandle, ref optionsInternal, clientDataAddress, friendsUpdateHandlerInternal);

			Helper.Dispose(ref optionsInternal);

			Helper.AssignNotificationIdToCallback(clientDataAddress, funcResult);

			return funcResult;
		}

		/// <summary>
		/// Retrieves the Epic Account ID of an entry from the blocklist that has already been retrieved by the <see cref="QueryFriends" /> API.
		/// <seealso cref="QueryFriends" />
		/// <seealso cref="GetBlockedUsersCount" />
		/// </summary>
		/// <param name="options">structure containing the Epic Account ID of the owner of the blocklist and the index into the list.</param>
		/// <returns>
		/// the Epic Account ID of the blocked user. Note that if the index provided is out of bounds, the returned Epic Account ID will be a "null" account ID.
		/// </returns>
		public EpicAccountId GetBlockedUserAtIndex(ref GetBlockedUserAtIndexOptions options)
		{
			GetBlockedUserAtIndexOptionsInternal optionsInternal = new GetBlockedUserAtIndexOptionsInternal();
			optionsInternal.Set(ref options);

			var funcResult = Bindings.EOS_Friends_GetBlockedUserAtIndex(InnerHandle, ref optionsInternal);

			Helper.Dispose(ref optionsInternal);

			EpicAccountId funcResultReturn;
			Helper.Get(funcResult, out funcResultReturn);
			return funcResultReturn;
		}

		/// <summary>
		/// Retrieves the number of blocked users on the blocklist that has already been retrieved by the <see cref="QueryFriends" /> API.
		/// <seealso cref="QueryFriends" />
		/// </summary>
		/// <param name="options">structure containing the Epic Account ID of user who owns the blocklist.</param>
		/// <returns>
		/// the number of users on the blocklist.
		/// </returns>
		public int GetBlockedUsersCount(ref GetBlockedUsersCountOptions options)
		{
			GetBlockedUsersCountOptionsInternal optionsInternal = new GetBlockedUsersCountOptionsInternal();
			optionsInternal.Set(ref options);

			var funcResult = Bindings.EOS_Friends_GetBlockedUsersCount(InnerHandle, ref optionsInternal);

			Helper.Dispose(ref optionsInternal);

			return funcResult;
		}

		/// <summary>
		/// Retrieves the Epic Account ID of an entry from the friends list that has already been retrieved by the <see cref="QueryFriends" /> API.
		/// The Epic Account ID returned by this function may belong to an account that has been invited to be a friend or that has invited the local user to be a friend.
		/// To determine if the Epic Account ID returned by this function is a friend or a pending friend invitation, use the <see cref="GetStatus" /> function.
		/// <seealso cref="GetFriendsCount" />
		/// <seealso cref="GetStatus" />
		/// </summary>
		/// <param name="options">structure containing the Epic Account ID of the owner of the friends list and the index into the list</param>
		/// <returns>
		/// the Epic Account ID of the friend. Note that if the index provided is out of bounds, the returned Epic Account ID will be a "null" account ID.
		/// </returns>
		public EpicAccountId GetFriendAtIndex(ref GetFriendAtIndexOptions options)
		{
			GetFriendAtIndexOptionsInternal optionsInternal = new GetFriendAtIndexOptionsInternal();
			optionsInternal.Set(ref options);

			var funcResult = Bindings.EOS_Friends_GetFriendAtIndex(InnerHandle, ref optionsInternal);

			Helper.Dispose(ref optionsInternal);

			EpicAccountId funcResultReturn;
			Helper.Get(funcResult, out funcResultReturn);
			return funcResultReturn;
		}

		/// <summary>
		/// Retrieves the number of friends on the friends list that has already been retrieved by the <see cref="QueryFriends" /> API.
		/// <seealso cref="GetFriendAtIndex" />
		/// </summary>
		/// <param name="options">structure containing the Epic Account ID of user who owns the friends list</param>
		/// <returns>
		/// the number of friends on the list
		/// </returns>
		public int GetFriendsCount(ref GetFriendsCountOptions options)
		{
			GetFriendsCountOptionsInternal optionsInternal = new GetFriendsCountOptionsInternal();
			optionsInternal.Set(ref options);

			var funcResult = Bindings.EOS_Friends_GetFriendsCount(InnerHandle, ref optionsInternal);

			Helper.Dispose(ref optionsInternal);

			return funcResult;
		}

		/// <summary>
		/// Retrieve the friendship status between the local user and another user.
		/// <seealso cref="FriendsStatus" />
		/// </summary>
		/// <param name="options">structure containing the Epic Account ID of the friend list to check and the account of the user to test friendship status</param>
		/// <returns>
		/// A value indicating whether the two accounts have a friendship, pending invites in either direction, or no relationship
		/// <see cref="FriendsStatus.Friends" /> is returned for two users that have confirmed friendship
		/// <see cref="FriendsStatus.InviteSent" /> is returned when the local user has sent a friend invitation but the other user has not accepted or rejected it
		/// <see cref="FriendsStatus.InviteReceived" /> is returned when the other user has sent a friend invitation to the local user
		/// <see cref="FriendsStatus.NotFriends" /> is returned when there is no known relationship
		/// </returns>
		public FriendsStatus GetStatus(ref GetStatusOptions options)
		{
			GetStatusOptionsInternal optionsInternal = new GetStatusOptionsInternal();
			optionsInternal.Set(ref options);

			var funcResult = Bindings.EOS_Friends_GetStatus(InnerHandle, ref optionsInternal);

			Helper.Dispose(ref optionsInternal);

			return funcResult;
		}

		/// <summary>
		/// Starts an asynchronous task that reads the user's friends list and blocklist from the backend service, caching it for future use.
		/// When the Social Overlay is enabled then this will be called automatically. The Social Overlay is enabled by default (see EOS_PF_DISABLE_SOCIAL_OVERLAY).
		/// </summary>
		/// <param name="options">structure containing the account for which to retrieve the friends list</param>
		/// <param name="clientData">arbitrary data that is passed back to you in the CompletionDelegate</param>
		/// <param name="completionDelegate">a callback that is fired when the async operation completes, either successfully or in error</param>
		public void QueryFriends(ref QueryFriendsOptions options, object clientData, OnQueryFriendsCallback completionDelegate)
		{
			QueryFriendsOptionsInternal optionsInternal = new QueryFriendsOptionsInternal();
			optionsInternal.Set(ref options);

			var clientDataAddress = System.IntPtr.Zero;

			var completionDelegateInternal = new OnQueryFriendsCallbackInternal(OnQueryFriendsCallbackInternalImplementation);
			Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);

			Bindings.EOS_Friends_QueryFriends(InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);

			Helper.Dispose(ref optionsInternal);
		}

		/// <summary>
		/// Starts an asynchronous task that rejects a friend invitation from another user. The completion delegate is executed after the backend response has been received.
		/// </summary>
		/// <param name="options">structure containing the logged in account and the inviting account</param>
		/// <param name="clientData">arbitrary data that is passed back to you in the CompletionDelegate</param>
		/// <param name="completionDelegate">a callback that is fired when the async operation completes, either successfully or in error</param>
		public void RejectInvite(ref RejectInviteOptions options, object clientData, OnRejectInviteCallback completionDelegate)
		{
			RejectInviteOptionsInternal optionsInternal = new RejectInviteOptionsInternal();
			optionsInternal.Set(ref options);

			var clientDataAddress = System.IntPtr.Zero;

			var completionDelegateInternal = new OnRejectInviteCallbackInternal(OnRejectInviteCallbackInternalImplementation);
			Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);

			Bindings.EOS_Friends_RejectInvite(InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);

			Helper.Dispose(ref optionsInternal);
		}

		/// <summary>
		/// Stop listening for blocklist changes on a previously bound handler.
		/// </summary>
		/// <param name="notificationId">The previously bound notification ID.</param>
		public void RemoveNotifyBlockedUsersUpdate(ulong notificationId)
		{
			Bindings.EOS_Friends_RemoveNotifyBlockedUsersUpdate(InnerHandle, notificationId);

			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		/// <summary>
		/// Stop listening for friends changes on a previously bound handler.
		/// </summary>
		/// <param name="notificationId">The previously bound notification ID.</param>
		public void RemoveNotifyFriendsUpdate(ulong notificationId)
		{
			Bindings.EOS_Friends_RemoveNotifyFriendsUpdate(InnerHandle, notificationId);

			Helper.RemoveCallbackByNotificationId(notificationId);
		}

		/// <summary>
		/// Starts an asynchronous task that sends a friend invitation to another user. The completion delegate is executed after the backend response has been received.
		/// It does not indicate that the target user has responded to the friend invitation.
		/// </summary>
		/// <param name="options">structure containing the account to send the invite from and the account to send the invite to</param>
		/// <param name="clientData">arbitrary data that is passed back to you in the CompletionDelegate</param>
		/// <param name="completionDelegate">a callback that is fired when the async operation completes, either successfully or in error</param>
		public void SendInvite(ref SendInviteOptions options, object clientData, OnSendInviteCallback completionDelegate)
		{
			SendInviteOptionsInternal optionsInternal = new SendInviteOptionsInternal();
			optionsInternal.Set(ref options);

			var clientDataAddress = System.IntPtr.Zero;

			var completionDelegateInternal = new OnSendInviteCallbackInternal(OnSendInviteCallbackInternalImplementation);
			Helper.AddCallback(out clientDataAddress, clientData, completionDelegate, completionDelegateInternal);

			Bindings.EOS_Friends_SendInvite(InnerHandle, ref optionsInternal, clientDataAddress, completionDelegateInternal);

			Helper.Dispose(ref optionsInternal);
		}

		[MonoPInvokeCallback(typeof(OnAcceptInviteCallbackInternal))]
		internal static void OnAcceptInviteCallbackInternalImplementation(ref AcceptInviteCallbackInfoInternal data)
		{
			OnAcceptInviteCallback callback;
			AcceptInviteCallbackInfo callbackInfo;
			if (Helper.TryGetAndRemoveCallback(ref data, out callback, out callbackInfo))
			{
				callback(ref callbackInfo);
			}
		}

		[MonoPInvokeCallback(typeof(OnBlockedUsersUpdateCallbackInternal))]
		internal static void OnBlockedUsersUpdateCallbackInternalImplementation(ref OnBlockedUsersUpdateInfoInternal data)
		{
			OnBlockedUsersUpdateCallback callback;
			OnBlockedUsersUpdateInfo callbackInfo;
			if (Helper.TryGetCallback(ref data, out callback, out callbackInfo))
			{
				callback(ref callbackInfo);
			}
		}

		[MonoPInvokeCallback(typeof(OnFriendsUpdateCallbackInternal))]
		internal static void OnFriendsUpdateCallbackInternalImplementation(ref OnFriendsUpdateInfoInternal data)
		{
			OnFriendsUpdateCallback callback;
			OnFriendsUpdateInfo callbackInfo;
			if (Helper.TryGetCallback(ref data, out callback, out callbackInfo))
			{
				callback(ref callbackInfo);
			}
		}

		[MonoPInvokeCallback(typeof(OnQueryFriendsCallbackInternal))]
		internal static void OnQueryFriendsCallbackInternalImplementation(ref QueryFriendsCallbackInfoInternal data)
		{
			OnQueryFriendsCallback callback;
			QueryFriendsCallbackInfo callbackInfo;
			if (Helper.TryGetAndRemoveCallback(ref data, out callback, out callbackInfo))
			{
				callback(ref callbackInfo);
			}
		}

		[MonoPInvokeCallback(typeof(OnRejectInviteCallbackInternal))]
		internal static void OnRejectInviteCallbackInternalImplementation(ref RejectInviteCallbackInfoInternal data)
		{
			OnRejectInviteCallback callback;
			RejectInviteCallbackInfo callbackInfo;
			if (Helper.TryGetAndRemoveCallback(ref data, out callback, out callbackInfo))
			{
				callback(ref callbackInfo);
			}
		}

		[MonoPInvokeCallback(typeof(OnSendInviteCallbackInternal))]
		internal static void OnSendInviteCallbackInternalImplementation(ref SendInviteCallbackInfoInternal data)
		{
			OnSendInviteCallback callback;
			SendInviteCallbackInfo callbackInfo;
			if (Helper.TryGetAndRemoveCallback(ref data, out callback, out callbackInfo))
			{
				callback(ref callbackInfo);
			}
		}
	}
}