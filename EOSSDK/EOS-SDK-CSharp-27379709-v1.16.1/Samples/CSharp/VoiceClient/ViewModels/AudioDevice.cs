// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.ViewModels
{
	public class AudioDevice : Bindable
	{
		private string m_Id;
		public string Id
		{
			get { return m_Id; }
			set { SetProperty(ref m_Id, value); }
		}

		private string m_Name;
		public string Name
		{
			get { return m_Name; }
			set { SetProperty(ref m_Name, value); }
		}

		private bool m_IsDefault;
		public bool IsDefault
		{
			get { return m_IsDefault; }
			set { SetProperty(ref m_IsDefault, value); }
		}

		public AudioDevice()
		{
		}
	}
}
