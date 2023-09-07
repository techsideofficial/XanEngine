// Copyright Epic Games, Inc. All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace Epic.OnlineServices.Samples.Views.Controls
{
	public class MaskedTextBox : TextBox
	{
		private bool m_IsMasking = false;

		public static readonly DependencyProperty RealTextProperty = DependencyProperty.Register("RealText", typeof(string), typeof(MaskedTextBox), new UIPropertyMetadata(null, OnRealTextChanged));

		public string RealText
		{
			get { return (string)GetValue(RealTextProperty); }
			set { SetValue(RealTextProperty, value); }
		}

		private static void OnRealTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var maskedTextBox = obj as MaskedTextBox;
			if (maskedTextBox != null)
			{
				maskedTextBox.Mask();
			}
		}

		public static readonly DependencyProperty IsMaskedProperty = DependencyProperty.Register("IsMasked", typeof(bool), typeof(MaskedTextBox), new UIPropertyMetadata(false, OnIsMaskedChanged));

		public bool IsMasked
		{
			get { return (bool)GetValue(IsMaskedProperty); }
			set { SetValue(IsMaskedProperty, value); }
		}

		private static void OnIsMaskedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var maskedTextBox = obj as MaskedTextBox;
			if (maskedTextBox != null && e.OldValue != e.NewValue)
			{
				maskedTextBox.RealText = "";
			}
		}

		public static readonly DependencyProperty MaskCharacterProperty = DependencyProperty.Register("MaskCharacter", typeof(char), typeof(MaskedTextBox), new UIPropertyMetadata('•', OnMaskCharacterChanged));

		public char MaskCharacter
		{
			get { return (char)GetValue(MaskCharacterProperty); }
			set { SetValue(MaskCharacterProperty, value); }
		}

		private static void OnMaskCharacterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var maskedTextBox = obj as MaskedTextBox;
			if (maskedTextBox != null && e.OldValue != e.NewValue)
			{
				maskedTextBox.Mask();
			}
		}

		private void Mask()
		{
			m_IsMasking = true;
			if (IsMasked && RealText != null)
			{
				Text = new string(MaskCharacter, RealText.Length);
			}
			else
			{
				Text = RealText;
			}
			m_IsMasking = false;
		}

		public MaskedTextBox()
		{
			TextChanged += MaskedTextBox_TextChanged;
		}

		private void MaskedTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var maskedTextBox = sender as MaskedTextBox;
			if (maskedTextBox != null && !maskedTextBox.m_IsMasking)
			{
				var selectionStart = maskedTextBox.SelectionStart;
				var selectionLength = maskedTextBox.SelectionLength;

				var realText = maskedTextBox.RealText;
				var text = maskedTextBox.Text;
				foreach (var change in e.Changes)
				{
					if (change.RemovedLength > 0)
					{
						realText = realText.Remove(change.Offset, change.RemovedLength);
					}

					if (change.AddedLength > 0)
					{
						realText = realText.Insert(change.Offset, text.Substring(change.Offset, change.AddedLength));
					}
				}

				maskedTextBox.RealText = realText;

				maskedTextBox.SelectionStart = selectionStart;
				maskedTextBox.SelectionLength = selectionLength;

				e.Handled = true;
			}
		}
	}
}
