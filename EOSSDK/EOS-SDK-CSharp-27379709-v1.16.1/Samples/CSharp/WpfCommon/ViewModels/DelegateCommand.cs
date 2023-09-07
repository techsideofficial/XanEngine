// Copyright Epic Games, Inc. All Rights Reserved.

using System;
using System.Windows.Input;

namespace Epic.OnlineServices.Samples.ViewModels
{
	public class DelegateCommand : ICommand
	{
		private Action<object> m_ExecuteMethod;

		public DelegateCommand(Action<object> executeMethod)
		{
			if (executeMethod == null)
			{
				throw new ArgumentNullException();
			}

			m_ExecuteMethod = executeMethod;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			m_ExecuteMethod(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, System.EventArgs.Empty);
			}
		}
	}
}
