// Copyright Epic Games, Inc. All Rights Reserved.

namespace Epic.OnlineServices.Samples.ViewModels
{
	public class LogMessage : Bindable
	{
		private string m_Text;
		public string Text
		{
			get { return m_Text; }
			set { SetProperty(ref m_Text, value); }
		}

		private LogStyle m_Style;
		public LogStyle Style
		{
			get { return m_Style; }
			set { SetProperty(ref m_Style, value); }
		}
	}
}
