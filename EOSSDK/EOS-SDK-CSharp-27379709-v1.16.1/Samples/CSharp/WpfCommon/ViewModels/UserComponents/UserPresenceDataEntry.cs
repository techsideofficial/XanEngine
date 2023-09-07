// Copyright Epic Games, Inc. All Rights Reserved.

using System;

namespace Epic.OnlineServices.Samples.ViewModels.UserComponents
{
	public class UserPresenceDataEntry : Bindable
	{
		private string m_Key;
		public string Key
		{
			get { return m_Key; }
			set { SetProperty(ref m_Key, value); }
		}

		private string m_Value;
		public string Value
		{
			get { return m_Value; }
			set { SetProperty(ref m_Value, value); }
		}

		public DelegateCommand RemoveCommand { get; private set; }

		public event EventHandler RemoveRequested;

		public UserPresenceDataEntry()
		{
			RemoveCommand = new DelegateCommand((parameter) =>
			{
				if (RemoveRequested != null)
				{
					RemoveRequested(this, null);
				}
			});
		}
	}
}