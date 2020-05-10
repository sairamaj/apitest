using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using ApiManager.Model;
using ApiManager.PopUp.ViewModels;
using ApiManager.PopUp.Views;
using ApiManager.Repository;
using ApiManager.ScenarioEditing;
using ApiManager.ScenarioEditing.CommandEditing.ViewModel;
using ApiManager.ScenarioEditing.CommandEditing.Views;
using ApiManager.ScenarioEditing.Models;
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
				var builder = new ContainerBuilder();
				builder.RegisterModule(new RegistrationModule());

				var serviceLocator = ServiceLocatorFactory.Create(builder);
				ServiceLocator.Initialize(serviceLocator);      // todo: need to revisit this (added to avoid passing locator to all ctors)
				TestSmartEditor();

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
			//var win = new EditPrintCommandWindow();
			//var item = new CommandScenarioItem("!print hello");
			//var vm = new EditPrintCommandViewModel(win, item);
			//win.DataContext = vm;
			//win.ShowDialog();
			//MessageBox.Show(item.Command);
			//System.Environment.Exit(-1);
			return;
			//var vm = new ApiResponsePoupViewModel("http://localhost/restapi/foo/bar", File.ReadAllText(@"c:\temp\test.json"), File.ReadAllText(@"c:\temp\test.json"));
			//var win = new ApiResponsePoupWindow { DataContext = vm };
			//win.ShowDialog();
			//System.Environment.Exit(-1);
			//Tes,tExtractWindow();
			//System.Environment.Exit(-1);
			//return;
			//TestAssertWindow();
			//System.Environment.Exit(-1);
			//return;
			//var win1 = new NewApiResourceWindow();
			//var vm1 = new NewApiResourceViewModel(win1,"Post", ServiceLocator.Locator.Resolve<IResourceManager>());
			//win1.DataContext = vm1;
			//if (win1.ShowDialog().Value)
			//{
			//	MessageBox.Show(vm1.Name);
			//}
			//System.Environment.Exit(-1);

			//var win = new EditApiCommandWindow();
			//var vm = new EditApiCommandViewModel(
			//	win,
			//	new ScenarioEditing.Models.ApiScenarioItem("api.foo post foo.txt"),
			//	ServiceLocator.Locator.Resolve<IResourceManager>()
			//	);
			//win.DataContext = vm;
			//win.ShowDialog();
			////MessageBox.Show(vm.Command);
			//System.Environment.Exit(-1);
			//return;


			var apiCmdInfo = new ApiCommandInfo();
			apiCmdInfo.ApiCommands = new List<ApiCommand>
			{
				new ApiCommand{
					Name = "api",
					Description = "api description here",
					Routes = new List<ApiRoute>{
						new ApiRoute{
							Name = "_",
							Path = "/",
							Description = "api root"
						},
						new ApiRoute{
							Name = "foo",
							Path = "/bar",
							Description = "api foo help"
						}
					}
				}
			};
			var bangCommands = new BangCommandInfo
			{
				BangCommands = new List<BangCommand>
				{
					new BangCommand {
						Name = "!extract",
						Description = "extract help here"
					},
					new BangCommand {
						Name = "!assert",
						Description = "assert help here"
					}
				}
			};
			var funcCommandInfo = new FunctionCommandInfo
			{
				Functions = new List<FunctionCommand>
				{
					new FunctionCommand{
						Name = "func1",
						Description = "func1 help here"
					},
					new FunctionCommand{
						Name = "func2",
						Description = "func2 help here"
					},
				}
			};

			var dynamicVariablesInfo = new DynamicVariableInfo()
			{
				DynamicVariables = new List<DynamicVariable>
				{
					new DynamicVariable{
						Name = "random",
						Description = "random help here"
					},
					new DynamicVariable{
						Name = "today_date",
						Description = "today_date help here"
					},
					new DynamicVariable{
						Name = "guid",
						Description = "guid help here"
					}
				}
			};

			EditorWindow editorWindow = new EditorWindow();
			var scenario = new Scenario(@"Configuration\Apis\Apigee\scenarios\list_apis\list.txt");
			editorWindow.DataContext = new ScenarioEditorViewModel(
				editorWindow,
				scenario,
				bangCommands,
				apiCmdInfo, funcCommandInfo, dynamicVariablesInfo);
			editorWindow.ShowDialog();
			System.Environment.Exit(-1);
		}

		private void TestAssertWindow()
		{
			var win = new EditAssertCommandWindow();
			var vm = new EditAssertCommandViewModel(win, new CommandScenarioItem("!assert"));
			win.DataContext = vm;
			if (win.ShowDialog().Value)
			{
				MessageBox.Show(vm.Command);
			}
		}
		private void TestExtractWindow()
		{
			var win = new EditExtractCommandWindow();
			var vm = new EditExtractCommandViewModel(win, new CommandScenarioItem("!extract"));
			win.DataContext = vm;
			if (win.ShowDialog().Value)
			{
				MessageBox.Show(vm.Command);
			}
		}
	}
}
