using System.IO;
using ApiManager.Scripts.ViewModels;
using ApiManager.Scripts.Views;

namespace ApiManager.Scripts
{
	internal static class JavaScriptUiHelper
	{
		public static void ShowJavaScriptWindow(string scriptFileName)
		{
			var scriptContent = string.Empty;
			if (File.Exists(scriptFileName))
			{
				scriptContent = File.ReadAllText(scriptFileName);
			}
			else
			{
				scriptContent = $"{scriptFileName} not found!";
			}

			var scriptViewModel = new JavaScriptViewModel(scriptContent, Path.GetFileName(scriptFileName));
			var win = new JavaScriptWindow { DataContext = scriptViewModel };
			win.ShowDialog();
		}
	}
}
