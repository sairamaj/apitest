using System.Windows;
using ApiManager.ViewModels;

namespace ApiManager
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			var win = new MainWindow { DataContext = new MainViewModel() };
			win.ShowDialog();
		}
	}
}
