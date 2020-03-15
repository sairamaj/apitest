using System;
using System.Diagnostics;
using System.Windows;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.ViewModels;
using Autofac;
using Wpf.Util.Core.Extensions;
using Wpf.Util.Core.Registration;

namespace ApiManager
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			this.DispatcherUnhandledException += (s, ex) =>
			{
				Trace.WriteLine(ex.Exception.GetExceptionDetails());
				MessageBox.Show(ex.Exception.GetExceptionDetails());
				System.Environment.Exit(-1);
			};

			try
			{
				var builder = new ContainerBuilder();
				var settings = new Settings
				{
					PythonPath = @"c:\\python37\\python",
					ApiTestPath = @"C:\sai\dev\apitest\src\ApiTester"
				};

				var apiExecutor = new ApiExecutor(settings);
				builder.RegisterInstance(apiExecutor).As<IApiExecutor>();
				builder.RegisterType<DataRepository>().As<IDataRepository>();
				var serviceLocator = ServiceLocatorFactory.Create(builder);

				var win = new MainWindow { DataContext = new MainViewModel(
					serviceLocator.Resolve<IApiExecutor>() ,
					serviceLocator.Resolve<IDataRepository>()
				)};
				win.ShowDialog();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.GetExceptionDetails());
			}
		}
	}
}
