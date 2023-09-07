// Copyright Epic Games, Inc. All Rights Reserved.

using System.Windows.Data;

namespace Epic.OnlineServices.Samples.Views.Converters
{
	public class EnumComparisonConverter : IValueConverter
	{
		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null && parameter != null && value.GetType().IsEnum && value.GetType() == parameter.GetType())
			{
				return (int)value == (int)parameter;
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}