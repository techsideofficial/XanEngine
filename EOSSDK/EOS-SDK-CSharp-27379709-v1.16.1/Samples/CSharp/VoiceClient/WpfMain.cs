// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Samples.ViewModels;
using Epic.OnlineServices.Samples.ViewModels.Menus;
using Epic.OnlineServices.Samples.Views.Windows;
using System;

namespace Epic.OnlineServices.Samples
{
	public static class WpfMain
	{
		[STAThread]
		static int Main()
		{
			bool requiresConnect = true;
			return new PlatformApplication().Run<MainWindow, UserVoiceMenu>(requiresConnect);
		}
	}
}
