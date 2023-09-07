// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Presence;
using Epic.OnlineServices.Samples.ViewModels.UserComponents;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Epic.OnlineServices.Samples.ViewModels.Menus
{
	public class UserPresenceMenu : UserComponentMenu<UserPresenceComponent>
	{
		private Status m_Status;
		public Status Status
		{
			get { return m_Status; }
			set { SetProperty(ref m_Status, value); }
		}

		private string m_RichText;
		public string RichText
		{
			get { return m_RichText; }
			set { SetProperty(ref m_RichText, value); }
		}

		private ObservableCollection<UserPresenceDataEntry> m_DataEntries;
		public ObservableCollection<UserPresenceDataEntry> DataEntries
		{
			get { return m_DataEntries; }
			set { SetProperty(ref m_DataEntries, value); }
		}

		public DelegateCommand AddEntryCommand { get; private set; }
		public DelegateCommand SubmitCommand { get; private set; }

		public UserPresenceMenu()
		{
			DataEntries = new ObservableCollection<UserPresenceDataEntry>();

			AddEntryCommand = new DelegateCommand((parameter) =>
			{
				AddDataEntry();
			});

			SubmitCommand = new DelegateCommand((parameter) =>
			{
				Submit();
			});
		}

		public void AddDataEntry()
		{
			var dataEntry = new UserPresenceDataEntry();
			dataEntry.RemoveRequested += DataEntry_RemoveRequested;
			DataEntries.Add(dataEntry);
		}

		private void DataEntry_RemoveRequested(object sender, EventArgs e)
		{
			var dataEntry = sender as UserPresenceDataEntry;
			dataEntry.RemoveRequested -= DataEntry_RemoveRequested;
			DataEntries.Remove(dataEntry);
		}

		public void Submit()
		{
			if (UserComponent == null)
			{
				return;
			}

			UserComponent.Modify(new UserPresenceModification()
			{
				Status = Status,
				RichText = RichText,
				DataEntries = DataEntries.Any() ? DataEntries.ToArray() : null
			});
		}
	}
}