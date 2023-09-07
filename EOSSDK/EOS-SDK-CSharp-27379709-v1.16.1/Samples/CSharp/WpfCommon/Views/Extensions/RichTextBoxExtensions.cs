// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Samples.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Epic.OnlineServices.Samples.Views.Extensions
{
	public static class RichTextBoxExtensions
	{
		public static readonly DependencyProperty InlinesSourceProperty = DependencyProperty.RegisterAttached("InlinesSource", typeof(ObservableCollection<LogMessage>), typeof(RichTextBoxExtensions), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnInlinesSourceChanged));

		public static ObservableCollection<LogMessage> GetInlinesSource(RichTextBox target)
		{
			return target.GetValue(InlinesSourceProperty) as ObservableCollection<LogMessage>;
		}

		public static void SetInlinesSource(RichTextBox target, ObservableCollection<LogMessage> value)
		{
			target.SetValue(InlinesSourceProperty, value);
		}

		private static Dictionary<RichTextBox, ObservableCollection<LogMessage>> s_RichTextBoxTextPartCollections = new Dictionary<RichTextBox, ObservableCollection<LogMessage>>();

		private static void OnInlinesSourceChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			if (target is RichTextBox richTextBox)
			{
				if (e.OldValue is ObservableCollection<LogMessage> oldCollection)
				{
					oldCollection.CollectionChanged -= CollectionChanged;
				}

				if (e.NewValue is ObservableCollection<LogMessage> newCollection)
				{
					s_RichTextBoxTextPartCollections[richTextBox] = newCollection;
					newCollection.CollectionChanged += CollectionChanged;

					if (richTextBox.Document == null)
					{
						richTextBox.Document = new FlowDocument();
					}

					richTextBox.Document.Blocks.Clear();
					AddItems(richTextBox, newCollection);
				}
			}
		}

		private static void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			foreach (var keyValuePair in s_RichTextBoxTextPartCollections)
			{
				if (keyValuePair.Value == sender)
				{
					AddItems(keyValuePair.Key, e.NewItems.Cast<LogMessage>());
				}
			}
		}

		private static void AddItems(RichTextBox target, IEnumerable<LogMessage> textParts)
		{
			if (textParts != null)
			{
				foreach (var textPart in textParts)
				{
					var paragraph = new Paragraph();
					var run = new Run();
					paragraph.Inlines.Add(run);
					run.DataContext = textPart;
					run.Text = textPart.Text;

					target.Document.Blocks.Add(paragraph);
				}

				if (target.VerticalOffset + target.ViewportHeight >= target.ExtentHeight)
				{
					target.ScrollToEnd();
				}
			}
		}
	}
}
