// Copyright Epic Games, Inc. All Rights Reserved.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Epic.OnlineServices.Samples.Views.Converters
{
	public class StringVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string)
			{
				string stringValue = value as string;
				if (string.IsNullOrEmpty(stringValue))
				{
					return Visibility.Visible;
				}
				else
				{
					return Visibility.Collapsed;
				}
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}