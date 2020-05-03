using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.ScenarioEditing;
using ApiManager.ScenarioEditing.NewLineItem.Views;
using ApiManager.ScenarioEditing.ViewModels;
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
				TestSmartEditor();
				var builder = new ContainerBuilder();
				builder.RegisterModule(new RegistrationModule());

				var serviceLocator = ServiceLocatorFactory.Create(builder);
				ServiceLocator.Initialize(serviceLocator);      // todo: need to revisit this (added to avoid passing locator to all ctors)

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

		private void TestSmartEditor()
		{
			//var apiCmdInfo = new ApiCommandInfo();
			//apiCmdInfo.ApiCommands["accesstoken"] = new List<string>
			//{
			//	{"password" }
			//};
			//apiCmdInfo.ApiCommands["apis"] = new List<string>
			//{
			//	{"_" }
			//};
			//var view = new CreateScenarioLineItemView()
			//{
			//	DataContext = new ApiManager.ScenarioEditing.NewLineItem.ViewModels.MainViewModel(
			//		new List<BangCommandInfo>
			//		{
			//			new BangCommandInfo("!assert","Assert help here"),
			//			new BangCommandInfo("!extract","Extracts variable")
			//		},
			//		apiCmdInfo
			//		)
			//};
			//view.ShowDialog();
			EditorWindow editorWindow = new EditorWindow();
			var scenario = new Scenario(@"Configuration\Apis\Apigee\scenarios\list_apis\list.txt");
			editorWindow.DataContext = new ScenarioEditorViewModel(scenario, new string[] { "accesstoken.password", "apis._" });
			editorWindow.ShowDialog();
			System.Environment.Exit(-1);
		}
	}
}
