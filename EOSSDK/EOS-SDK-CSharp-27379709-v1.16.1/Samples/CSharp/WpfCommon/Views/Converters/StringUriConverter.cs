// Copyright Epic Games, Inc. All Rights Reserved.

using System;
using System.Windows.Data;

namespace Epic.OnlineServices.Samples.Views.Converters
{
	public class StringUriConverter : IValueConverter
	{
		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is string stringValue)
			{
				return new Uri(stringValue);
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}