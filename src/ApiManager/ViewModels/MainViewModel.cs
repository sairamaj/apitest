using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Asserts.ViewModels;
using ApiManager.Extensions;
using ApiManager.Model;
using ApiManager.Pipes;
using ApiManager.Repository;
using ApiManager.Resources.ViewModels;
using ApiManager.ScenarioEditing;
using ApiManager.ScenarioEditing.ViewModel;
using ApiManager.Scripts.ViewModels;
using ApiManager.Variables.ViewModels;
using Newtonsoft.Json;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.Extensions;
using Wpf.Util.Core.Registration;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class MainViewModel : CoreViewModel
	{
		private ApiViewModel _selectedApiInfoViewModel;
		private ICommandExecutor _apiExecutor;
		private IDataRepository _dataRepository;
		private ISettings _settings;
		private EnvironmentViewModel _selectedEnvironment;
		private IResourceManager _resourceManager;
		private IServiceLocator _serviceLocator;

		public MainViewModel(
			ICommandExecutor executor,
			IDataRepository dataRepository,
			ISettings settings,
			IResourceManager resourceManager,
			IServiceLocator serviceLocator)
		{
			this._apiExecutor = executor ?? throw new ArgumentNullException(nameof(executor));
			this._dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
			this._resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
			this._serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));

			this.ApiInfoViewModels = new SafeObservableCollection<ApiViewModel>();
			this.RunCommand = new DelegateCommand(async () => await this.RunAsync());
			this.OpenCommandPrompt = new DelegateCommand(async () => await this.OpenCommandPromptAsync());
			this.ShowIssuesCommand = new DelegateCommand(this.ShowIssues);
			this.RefreshCommand = new DelegateCommand(this.Load);
			this.NewScenarioFileCommand = new DelegateCommand(() =>
			{
				var selectedScenarioViewModel = this.GetSelectedScenario();
				if (selectedScenarioViewModel == null)
				{
					MessageBox.Show("Select scneario folder/file.");
					return;
				}
				Action createNewScenario = () => CreateNewScenario(selectedScenarioViewModel);
				createNewScenario.WithErrorMessageBox();
			});

			this.NewScenarioFolderCommand = new DelegateCommand(() =>
			{
				var selectedScenarioViewModel = this.GetSelectedScenario();
				Action createNewScenarioContainer = () => CreateNewScenarioContainer(selectedScenarioViewModel);
				createNewScenarioContainer.WithErrorMessageBox();
			});

			this.NewRootScenarioFolderCommand = new DelegateCommand(() =>
			{
				Action createNewScenarioContainer = () => CreateNewScenarioContainer(null);
				createNewScenarioContainer.WithErrorMessageBox();
			});
			

			this.RefreshScenariosCommand = new DelegateCommand(() => { this.RefreshScenarios(true); });

			this.Load();
			try
			{
				// SubscribeApiInfo();
				// SubscribeLogs();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}

			this.LogViewModel = new LogViewModel();
			//this.Scenarios = this.SelectedApiInfoViewModel.ApiInfo.Scenarios.Select(c => new ScenarioViewModel(c, this.SelectedApiInfoViewModel.ApiInfo, this._dataRepository));
			//this.SelectedScneario = this.Scenarios.FirstOrDefault();
		}
		public ObservableCollection<ApiViewModel> ApiInfoViewModels { get; set; }

		public ApiViewModel SelectedApiInfoViewModel
		{
			get
			{
				return this._selectedApiInfoViewModel;
			}
			set
			{
				this._selectedApiInfoViewModel = value;
				this.CurrentRequestResponseViewModel = new RequestResponseContainerViewModel(this.SelectedApiInfoViewModel.RequestResponses);
				OnPropertyChanged(() => this.CurrentRequestResponseViewModel);
				this.OnApiConfigSelectionChange();
			}
		}

		public EnvironmentViewModel SelectedEnvironment
		{
			get
			{
				return this._selectedEnvironment;
			}
			set
			{
				this._selectedEnvironment = value;
				this.VariableContainerViewModel.UpdateEnvironmentVariables(this._selectedEnvironment.Environment);
			}
		}

		public ObservableCollection<CommandTreeViewModel> Scenarios { get; set; }
		public IEnumerable<EnvironmentViewModel> Environments { get; set; }
		public ICommand RunCommand { get; set; }
		public bool IsClearBeforeRun { get; set; }
		public ICommand OpenCommandPrompt { get; set; }
		public ICommand ShowIssuesCommand { get; set; }
		public ICommand RefreshCommand { get; set; }
		public ICommand NewScenarioFileCommand { get; set; }
		public ICommand NewScenarioFolderCommand { get; set; }
		public ICommand NewRootScenarioFolderCommand { get; set; }
		
		public ICommand RefreshScenariosCommand { get; set; }

		public RequestResponseContainerViewModel CurrentRequestResponseViewModel { get; set; }

		public LogViewModel LogViewModel { get; set; }
		public VariableContainerViewModel VariableContainerViewModel { get; private set; }
		public AssertContainerViewModel AssertContainerViewModel { get; private set; }
		public ScriptContainerViewModel ScriptContainerViewModel { get; private set; }
		public ResourceContainerViewModel PostResourceContainerViewModel { get; private set; }
		public ResourceContainerViewModel PatchResourceContainerViewModel { get; private set; }
		

		public async Task RunAsync()
		{
			if (this.SelectedApiInfoViewModel == null)
			{
				MessageBox.Show("Select Api", "Api", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedEnvironment == null)
			{
				MessageBox.Show("Select Environment ", "Environment", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			using (var apiCommunicator = this._serviceLocator.Resolve<IApiTestConsoleCommunicator>())
			{
				try
				{
					this.SubscribeApiInfo(apiCommunicator);

					var selectedScenario = GetSelectedScenario();

					if (this.IsClearBeforeRun)
					{
						this.CurrentRequestResponseViewModel?.Clear();
					}

					if (selectedScenario is ScenarioViewModel scenarioViewModel)
					{
						this.SelectedApiInfoViewModel.Add(new ApiExecuteInfo(
							this.SelectedApiInfoViewModel.Name, this.SelectedEnvironment.Environment, scenarioViewModel.Scenario), null);

						await new Executor(this._apiExecutor, this._settings)
										   .RunScenarioAsync(
										   this.SelectedApiInfoViewModel.ApiInfo,
										   this.SelectedEnvironment.Environment,
										   scenarioViewModel.Scenario).ConfigureAwait(false);
					}
					else if (selectedScenario is ScenarioContainerViewModel scenaroContainer)
					{
						foreach (var scenarioviewModel
							in scenaroContainer.Children.Flatten(c => c.Children.OfType<CommandTreeViewModel>()).OfType<ScenarioViewModel>())

						{
							this.SelectedApiInfoViewModel.Add(
								new ApiExecuteInfo(
								this.SelectedApiInfoViewModel.Name,
								this.SelectedEnvironment.Environment,
								scenarioviewModel.Scenario), null);

							await new Executor(this._apiExecutor, this._settings)
								.RunScenarioAsync(
								this.SelectedApiInfoViewModel.ApiInfo,
								this.SelectedEnvironment.Environment,
								scenarioviewModel.Scenario).ConfigureAwait(false);
						}
					}
					else
					{
						MessageBox.Show("Select Scenario File", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				catch (System.Exception e)
				{
					MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private CommandTreeViewModel GetSelectedScenario()
		{
			var allScenarioViewModels = this.Scenarios.Flatten(c => c.Children.OfType<CommandTreeViewModel>());
			foreach (var scenario in allScenarioViewModels.OfType<CommandTreeViewModel>())
			{
				if (scenario.IsSelected)
				{
					return scenario;
				}
			}

			return null;
		}

		private async Task OpenCommandPromptAsync()
		{
			if (this.SelectedApiInfoViewModel == null)
			{
				MessageBox.Show("Select Api", "Environment", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedEnvironment == null)
			{
				MessageBox.Show("Select Environment", "Environment", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				var apiInfo = this.SelectedApiInfoViewModel.ApiInfo;
				var result = await _apiExecutor.OpenCommandPromptAsync(
					new TestData
					{
						ConfigName = apiInfo.Configuration,
						VariablesFileName = this.SelectedEnvironment.FileName,
						SessionName = apiInfo.Name,
					}
					).ConfigureAwait(false);
			}
			catch (System.Exception e)
			{
				MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private async void OnApiConfigSelectionChange()
		{
			this.Environments = this.SelectedApiInfoViewModel.ApiInfo.Environments
				.Select(v => new EnvironmentViewModel(this.SelectedApiInfoViewModel.ApiInfo, v, this._dataRepository));
			OnPropertyChanged(() => this.Environments);

			this.SelectedEnvironment = this.Environments.FirstOrDefault();
			OnPropertyChanged(() => this.SelectedEnvironment);

			this.VariableContainerViewModel.UpdateApiVariables(this.SelectedApiInfoViewModel?.ApiInfo);
			this.RefreshScenarios(false);
		}

		private void RefreshScenarios(bool isRefresh)
		{
			if (this.SelectedApiInfoViewModel == null)
			{
				MessageBox.Show("Selected Api", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			if (isRefresh)
			{
				this.SelectedApiInfoViewModel.ApiInfo.Scenarios = this._dataRepository.GetScenarios(this.SelectedApiInfoViewModel.ApiInfo);
			}

			var scenarioContainerViewModels = this.SelectedApiInfoViewModel.ApiInfo.Scenarios
				.Where(s => s.IsContainer)
				.Select(c => new ScenarioContainerViewModel(
					null,
					c,
					(e, s) => this.DoScenarioAction(e,s),
					this.SelectedApiInfoViewModel.ApiInfo,
					this._dataRepository))
				.ToList();
			this.Scenarios = new SafeObservableCollection<CommandTreeViewModel>(scenarioContainerViewModels.OfType<CommandTreeViewModel>().ToList());
			OnPropertyChanged(() => this.Scenarios);
		}
		private void SubscribeApiInfo(IApiTestConsoleCommunicator communicator)
		{
			communicator.Add("apiinfo", "api", msg =>
			{
				ConsumePipeData<ApiRequest>(msg);
			});

			communicator.Add("apiinfo", "extract", msg =>
			{
				ConsumePipeData<ExtractVariableInfo>(msg);
			});

			communicator.Add("apiinfo", "assert", msg =>
			{
				ConsumePipeData<AssertInfo>(msg);
			});

			communicator.Add("management", "apicommands", msg =>
			{
				ConsumeManagementPipeData<ApiCommandInfo>(msg);
			});

			communicator.Add("management", "commands", msg =>
			{
				ConsumeManagementPipeData<HelpCommandInfo>(msg);
			});
			communicator.Add("error", "error", msg =>
			{
				ConsumePipeData<ErrorInfo>(msg);
			});
			communicator.Add("js", "js", msg =>
			{
				ConsumePipeData<JsScriptInfo>(msg);
			});
		}

		private void ConsumePipeData<T>(string message) where T : Info
		{
			var info = JsonConvert.DeserializeObject<T>(message);
			if (info.Session == null)
			{
				return;
			}
			var apiName = info.Session.Split('|').First();
			var scenarioName = info.Session.Split('|').Last();
			var envFolder = this.ApiInfoViewModels.FirstOrDefault(eF => eF.Name == apiName);
			if (envFolder == null)
			{
				return;
			}

			envFolder.Add(info, scenarioName);
		}

		private void ConsumeManagementPipeData<T>(string message) where T : Info
		{
			var info = JsonConvert.DeserializeObject<T>(message);
			Console.WriteLine(info);
			this._dataRepository.AddManagementInfo(info);
		}

		//private void SubscribeLogs()
		//{
		//	Subscribe("log", msg =>
		//	{
		//		try
		//		{
		//			this.LogViewModel.Add(msg);
		//		}
		//		catch (Exception e)
		//		{
		//			MessageBox.Show(e.Message);
		//			TraceLogger.Error($"Error in apiinfo subscribe method {e.Message} ");
		//		}
		//	});
		//}

		//private void SubscribeManagement()
		//{
		//	Subscribe("management", msg =>
		//	{
		//		try
		//		{
		//			this.LogViewModel.Add(msg);
		//		}
		//		catch (Exception e)
		//		{
		//			MessageBox.Show(e.Message);
		//			TraceLogger.Error($"Error in apiinfo subscribe method {e.Message} ");
		//		}
		//	});
		//}

		private void ShowIssues()
		{
			try
			{
				Process.Start("issues.txt");
			}
			catch (Exception e)
			{
				MessageBox.Show($"{e.Message} - issues.txt", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Load()
		{
			this.ApiInfoViewModels.Clear();
			foreach (var envInfo in this._dataRepository.GetApiConfigurations())
			{
				this.ApiInfoViewModels.Add(new ApiViewModel(envInfo, this._dataRepository, this._apiExecutor));
			}

			this.VariableContainerViewModel = new VariableContainerViewModel(this._resourceManager);
			this.AssertContainerViewModel = new AssertContainerViewModel(this._resourceManager);
			this.ScriptContainerViewModel = new ScriptContainerViewModel(this._resourceManager);
			this.PostResourceContainerViewModel = new ResourceContainerViewModel(this._resourceManager, "post");
			this.PatchResourceContainerViewModel= new ResourceContainerViewModel(this._resourceManager, "patch");

			this.SelectedApiInfoViewModel = this.ApiInfoViewModels.FirstOrDefault();
			this.OnApiConfigSelectionChange();
		}

		private void CreateNewScenarioContainer(CommandTreeViewModel selectedScenarioViewModel)
		{
			ScenarioContainerViewModel container = null;
			if (selectedScenarioViewModel is ScenarioViewModel fileViewModel)
			{
				container = fileViewModel.Parent as ScenarioContainerViewModel;
			}
			if (selectedScenarioViewModel is ScenarioContainerViewModel)
			{
				container = selectedScenarioViewModel as ScenarioContainerViewModel;
			}

			var rootDirctory = string.Empty;
			if (container == null)
			{
				// root directory
				var firstContainer = this.Scenarios.OfType<ScenarioContainerViewModel>().FirstOrDefault();
				if (firstContainer == null)
				{
					MessageBox.Show("At least one container should have it(current limitation)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				rootDirctory = Path.GetDirectoryName(firstContainer.FileName);
			}
			else
			{
				rootDirctory = container.FileName;
			}

			var scenarioContainer = ScenarioEditingHelper.CreateNewScenarioContainer(rootDirctory);
			if (scenarioContainer == null)
			{
				return;
			}

			if (container != null)
			{
				container.AddScenarioContainer(scenarioContainer);
			}
			else
			{
				// adding to root container.
				this.Scenarios.Add(
					new ScenarioContainerViewModel(
						null, 
						scenarioContainer,
						(e, s) => this.DoScenarioAction(e,s),
						this.SelectedApiInfoViewModel.ApiInfo, 
						this._dataRepository));
			}
		}

		private void CreateNewScenario(CommandTreeViewModel selectedScenarioViewModel)
		{
			ScenarioContainerViewModel container = null;
			if (selectedScenarioViewModel is ScenarioViewModel fileViewModel)
			{
				container = fileViewModel.Parent as ScenarioContainerViewModel;
			}
			if (selectedScenarioViewModel is ScenarioContainerViewModel)
			{
				container = selectedScenarioViewModel as ScenarioContainerViewModel;
			}

			if (container == null)
			{
				MessageBox.Show($"Unable to find container view for :{selectedScenarioViewModel.Name}");
				return;
			}

			var scenario = ScenarioEditingHelper.CreateNewScenario(container.FileName);
			if (scenario == null)
			{
				return;
			}

			container.AddScenario(scenario);
		}

		private void DoScenarioAction(ScenarioAction e, Scenario scenario)
		{
			switch (e)
			{
				case ScenarioAction.Copy:
					break;
				case ScenarioAction.Delete:
					if (scenario.IsContainer)
					{
						var childToRemove = this.Scenarios.OfType<ScenarioContainerViewModel>().FirstOrDefault(s => s.Scenario.Name == scenario.Name);
						this.Scenarios.Remove(childToRemove);
					}
					else
					{
						var childToRemove = this.Scenarios.OfType<ScenarioViewModel>().FirstOrDefault(s => s.Scenario.Name == scenario.Name);
						this.Scenarios.Remove(childToRemove);
					}
					break;
			}
		}
	}
}
