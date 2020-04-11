using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				ShowErrorMessage(e.ToString(), title);
			}
		}

		public static void ShowErrorMessage(string title, string message)
		{
			MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

	}
}
