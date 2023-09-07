// Copyright Epic Games, Inc. All Rights Reserved.

using System.Windows.Data;

namespace Epic.OnlineServices.Samples.Views.Converters
{
	public class IsNullConverter : IValueConverter
	{
		public bool Inverse { get; set; }

		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (Inverse)
			{
				return value != null;
			}
			
			return value == null;
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}