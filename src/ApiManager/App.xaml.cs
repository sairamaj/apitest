using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using ApiManager.QuickTest.ViewModels;
using ApiManager.QuickTest.Views;
using ApiManager.Repository;
using ApiManager.ViewModels;
using ApiManager.Views;
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
				TestQuickEdit();
				var builder = new ContainerBuilder();
				builder.RegisterModule(new RegistrationModule());

				var serviceLocator = ServiceLocatorFactory.Create(builder);
				ServiceLocator.Initialize(serviceLocator);		// todo: need to revisit this (added to avoid passing locator to all ctors)

				var win = new MainWindow
				{
					DataContext = new MainViewModel(
					serviceLocator.Resolve<ICommandExecutor>(),
					serviceLocator.Resolve<IDataRepository>(),
					serviceLocator.Resolve<ISettings>(),
					serviceLocator.Resolve<IResourceManager>(),
					serviceLocator)
				};
				RunWithSingleInstance(() => win.ShowDialog());
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.GetExceptionDetails());
				System.Environment.Exit(-1);
			}
		}

		private void RunWithSingleInstance(Action action)
		{
			const string appGuid = "9570F800-8473-4E1E-8A09-471D0E133BEC";

			using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
			{
				if (!mutex.WaitOne(0, false))
				{
					MessageBox.Show(
						"Currently only one instance can be running. Please switch to the other instance.",
						"Error", 
						MessageBoxButton.OK, 
						MessageBoxImage.Error);
					System.Environment.Exit(-1);
				}
				action();
			}
		}

		private void TestQuickEdit()
		{
			QuickEditWindow quickEdit = new QuickEditWindow();
			var commands = new Dictionary<string, IEnumerable<string>>();
			commands["accesstoken"] = new List<string>
			{
				"password"
			};
			commands["apis"] = new List<string>
			{
				"_"
			};
			commands["developers"] = new List<string>
			{
				"_"
			};

			quickEdit.DataContext = new QuickEditorViewModel(new Model.ApiCommandInfo
			{
				ApiCommands = commands
			}, new Model.Environment("Apigee", @"Configuration\Apis\Apigee\config.json"));
			quickEdit.ShowDialog();
			System.Environment.Exit(-1);
		}
	}
}
