using System;
using System.Windows;

namespace ApiManager.Utils
{
	internal static class UiHelper
	{
		public static void SafeAction(Action a,string title)
		{
			try
			{
				a();
			}
			catch (Exception e)
			{
				ShowErrorMessage(title, e.ToString());
			}
		}

		public static void ShowErrorMessage(string title, string message)
		{
			MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

	}
}
