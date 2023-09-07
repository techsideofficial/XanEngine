// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Samples.ViewModels;
using Epic.OnlineServices.Samples.ViewModels.UserComponents;
using System.Windows.Data;

namespace Epic.OnlineServices.Samples.Views.Converters
{
	public class IsLocalUserAdminConverter : IValueConverter
	{
		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var platformApplication = value as PlatformApplication;

			if (platformApplication != null && platformApplication.FirstLocalUser != null)
			{
				return platformApplication.FirstLocalUser.GetOrAddComponent<UserVoiceComponent>().IsAdmin;
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}