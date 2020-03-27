using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using ApiManager.Model;
using ApiManager.Pipes;
using ApiManager.Repository;
using ApiManager.ViewModels;
using ApiManager.Views;
using Autofac;
using Newtonsoft.Json;
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
				var settings = JsonConvert.DeserializeObject<Settings>(
					File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json")));
				settings.ConsoleExecutableName = Path.GetFullPath(settings.ConsoleExecutableName);
				settings.WorkingDirectory= Path.GetFullPath(settings.WorkingDirectory);
				var apiExecutor = new ApiExecutor(settings);
				builder.RegisterInstance(apiExecutor).As<IApiExecutor>();
				builder.RegisterType<DataRepository>().As<IDataRepository>();
				builder.RegisterType<MessageListener>().As<IMessageListener>();
				// builder.RegisterType<FakeMessageListener>().As<IMessageListener>();
				var serviceLocator = ServiceLocatorFactory.Create(builder);

				var win = new MainWindow
				{
					DataContext = new MainViewModel(
					serviceLocator.Resolve<IApiExecutor>(),
					serviceLocator.Resolve<IDataRepository>(),
					serviceLocator.Resolve<IMessageListener>()
				)
				};
				win.ShowDialog();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.GetExceptionDetails());
				System.Environment.Exit(-1);
			}
		}

	}
}
