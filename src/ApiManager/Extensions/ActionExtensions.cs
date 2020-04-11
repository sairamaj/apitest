using System;
using System.Windows;

namespace ApiManager.Extensions
{
	internal static class ActionExtensions
	{
		public static void WithErrorMessageBox(this Action action)
		{
			try
			{
				action();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
