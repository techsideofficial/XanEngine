// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Samples.ViewModels;
using System.Windows.Data;

namespace Epic.OnlineServices.Samples.Views.Converters
{
	public class IsLocalUserConverter : IValueConverter
	{
		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var user = value as User;

			if (user != null)
			{
				return user.IsLocalUser;
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}