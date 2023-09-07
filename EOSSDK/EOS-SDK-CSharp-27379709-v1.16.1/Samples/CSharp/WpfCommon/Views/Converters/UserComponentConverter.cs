// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Samples.ViewModels;
using Epic.OnlineServices.Samples.ViewModels.UserComponents;
using System;
using System.Linq;
using System.Windows.Data;

namespace Epic.OnlineServices.Samples.Views.Converters
{
	public class UserComponentConverter : IValueConverter
	{
		private Type m_UserComponentType;
		public Type UserComponentType
		{
			get { return m_UserComponentType; }
			set
			{
				if (value.IsSubclassOf(typeof(UserComponent)))
				{
					m_UserComponentType = value;
				}
				else
				{
					throw new Exception($"'{value}' is not a subclass of UserComponent.");
				}
			}
		}
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is User user && m_UserComponentType != null)
			{
				return user.Components.FirstOrDefault(component => component.GetType() == m_UserComponentType);
			}

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
