// Copyright Epic Games, Inc. All Rights Reserved.

using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Samples
{
	internal class Program
	{
		private static void Main()
		{
			try
			{
				using (SimpleOverlayPurchasingApplication application = new SimpleOverlayPurchasingApplication())
				{
					application.Run();
				}
			}
			catch (Exception ex)
			{
				MessageBox(default, ex.Message, "Exception", MessageBoxType.MB_ICONERROR);
				throw;
			}
		}

		[Flags]
		private enum MessageBoxType : uint
		{
			MB_ICONERROR = 0x00000010,
		}

		[DllImport("user32.dll")]
		private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, MessageBoxType uType);
	}
}