using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApiManager.Views
{
	public static class InputBindingsManager
	{

		public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty = 
			DependencyProperty.RegisterAttached(
				"UpdatePropertySourceWhenEnterPressed", 
				typeof(ICommand), 
				typeof(InputBindingsManager), 
				new PropertyMetadata(null, OnUpdatePropertySourceWhenEnterPressedPropertyChanged));

		static InputBindingsManager()
		{

		}

		public static void SetUpdatePropertySourceWhenEnterPressed(DependencyObject dp, DependencyProperty value)
		{
			dp.SetValue(UpdatePropertySourceWhenEnterPressedProperty, value);
		}

		public static DependencyProperty GetUpdatePropertySourceWhenEnterPressed(DependencyObject dp)
		{
			return (DependencyProperty)dp.GetValue(UpdatePropertySourceWhenEnterPressedProperty);
		}

		private static void OnUpdatePropertySourceWhenEnterPressedPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			UIElement element = dp as UIElement;

			if (element == null)
			{
				return;
			}

			
			if (e.OldValue != null)
			{
				element.PreviewKeyDown -= HandlePreviewKeyDown;
			}

			if (e.NewValue != null)
			{
	element.PreviewKeyDown += new KeyEventHandler(HandlePreviewKeyDown);
			}
		}

		static void HandlePreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				UIElement element = (UIElement)sender;
				ICommand command = (ICommand)element.GetValue(InputBindingsManager.UpdatePropertySourceWhenEnterPressedProperty);
				if (command != null)
				{
					command.Execute(e);
				}
			}
		}
	}
}
