// Copyright Epic Games, Inc. All Rights Reserved.

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Epic.OnlineServices.Samples.Views.Converters
{
	public class EnumCollectionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
#if DEV
			bool isDevEnabled = true;
#else
			bool isDevEnabled = false;
#endif

			if (value is Auth.LoginCredentialType && !isDevEnabled)
			{
				var values = new Auth.LoginCredentialType[] { Auth.LoginCredentialType.Developer };
				return values.ToList();
			}
			else if (value != null && value.GetType().IsEnum)
			{
				// Hide the legacy DeviceCode from available login methods, as it is no longer supported by the SDK.
				return Enum.GetValues(value.GetType()).Cast<Enum>()
					.Where(type => !(type is Auth.LoginCredentialType) || (Auth.LoginCredentialType)type != Auth.LoginCredentialType.DeviceCode)
					.ToList();
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}