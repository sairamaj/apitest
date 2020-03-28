using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Pipes;
using ApiManager.Repository;
using Newtonsoft.Json;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class MainViewModel : CoreViewModel
	{
		private ApiViewModel _selectedApiInfoViewModel;
		private ScenarioViewModel _selectedScneario;
		private IMessageListener _listener;
		private ICommandExecutor _apiExecutor;
		private PipeDataProcessor _dataProcessor;
		private IDataRepository _dataRepository;

		public MainViewModel(
			ICommandExecutor executor,
			IDataRepository dataRepository,
			IMessageListener listener)
		{
			this._apiExecutor = executor ?? throw new ArgumentNullException(nameof(executor));
			this._listener = listener ?? throw new ArgumentNullException(nameof(listener));
			this._dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));

			this.ApiInfoViewModels = new SafeObservableCollection<ApiViewModel>();
			this.RunCommand = new DelegateCommand(async () => await this.RunAsync());
			this.OpenCommandPrompt = new DelegateCommand(async () => await this.OpenCommandPromptAsync());
			_dataProcessor = new PipeDataProcessor(listener, error =>
			{
				this.LogViewModel.Add(error);
			});
			this.ShowIssuesCommand = new DelegateCommand(this.ShowIssues);

			foreach (var envInfo in dataRepository.GetApiConfigurations())
			{
				this.ApiInfoViewModels.Add(new ApiViewModel(envInfo, dataRepository, executor));
			}

			try
			{
				SubscribeApiInfo();
				SubscribeLogs();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}

			this.SelectedApiInfoViewModel= this.ApiInfoViewModels.FirstOrDefault();
			this.LogViewModel = new LogViewModel();
			this.Scenarios = this.SelectedApiInfoViewModel.EnvironmentInfo.Scenarios.Select(c => new ScenarioViewModel(c));
			this.SelectedScneario = this.Scenarios.FirstOrDefault();
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
				this.Change();
			}
		}
		public ScenarioViewModel SelectedScneario
		{
			get
			{
				return this._selectedScneario;
			}
			set
			{
				this._selectedScneario = value;
				OnPropertyChanged(() => this.SelectedScneario);
			}
		}
		public EnvironmentViewModel SelectedEnvironment { get; set; }

		public IEnumerable<ScenarioViewModel> Scenarios { get; set; }
		public IEnumerable<EnvironmentViewModel> Environments { get; set; }
		public ICommand RunCommand { get; set; }
		public bool IsClearBeforeRun { get; set; }
		public ICommand OpenCommandPrompt { get; set; }
		public ICommand ShowIssuesCommand { get; set; }
		public RequestResponseContainerViewModel CurrentRequestResponseViewModel { get; set; }

		public LogViewModel LogViewModel { get; set; }
		public async Task RunAsync()
		{
			if (this.SelectedApiInfoViewModel == null)
			{
				MessageBox.Show("Select Api", "Api", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedScneario == null)
			{
				MessageBox.Show("Select Scenario File", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedEnvironment == null)
			{
				MessageBox.Show("Select Variable File", "Variable File", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				if (this.IsClearBeforeRun)
				{
					this.CurrentRequestResponseViewModel?.Clear();
				}

				var envInfo = this.SelectedApiInfoViewModel.EnvironmentInfo;
				var result = await _apiExecutor.StartAsync(
					new TestData
					{
						ConfigName = envInfo.Configuration,
						CommandsTextFileName = SelectedScneario.FileName,
						VariablesFileName = SelectedEnvironment.FileName,
						SessionName = envInfo.Name,
					}
					).ConfigureAwait(false);
			}
			catch (System.Exception e)
			{
				MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private async Task OpenCommandPromptAsync()
		{
			if (this.SelectedApiInfoViewModel == null)
			{
				MessageBox.Show("Select Api", "Environment", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedScneario == null)
			{
				MessageBox.Show("Select Scenario", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedEnvironment == null)
			{
				MessageBox.Show("Select Environment", "Environment", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				var envInfo = this.SelectedApiInfoViewModel.EnvironmentInfo;
				var result = await _apiExecutor.OpenCommandPromptAsync(
					new TestData
					{
						ConfigName = envInfo.Configuration,
						CommandsTextFileName = this.SelectedScneario.FileName,
						VariablesFileName = this.SelectedEnvironment.FileName,
						SessionName = envInfo.Name,
					}
					).ConfigureAwait(false);
			}
			catch (System.Exception e)
			{
				MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private async void Change()
		{
			this.Scenarios = this.SelectedApiInfoViewModel.EnvironmentInfo.Scenarios.Select(c => new ScenarioViewModel(c));
			this.Environments = this.SelectedApiInfoViewModel.EnvironmentInfo.Environments.Select(v => new EnvironmentViewModel(v));
			OnPropertyChanged(() => this.Scenarios);
			OnPropertyChanged(() => this.Environments);

			this.SelectedScneario = this.Scenarios.FirstOrDefault();
			this.SelectedEnvironment = this.Environments.FirstOrDefault();
			OnPropertyChanged(() => this.SelectedScneario);
			OnPropertyChanged(() => this.SelectedEnvironment);
		}

		private void Subscribe(string name, Action<string> onMessage)
		{
			try
			{
				this._listener.SubScribe(name, onMessage);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		private void SubscribeApiInfo()
		{
			_dataProcessor.Add("apiinfo", "api", msg =>
			{
				ConsumePipeData<ApiRequest>(msg);
			});

			_dataProcessor.Add("apiinfo", "extract", msg =>
			{
				ConsumePipeData<ExtractVariableInfo>(msg);
			});

			_dataProcessor.Add("apiinfo", "assert", msg =>
			{
				ConsumePipeData<AssertInfo>(msg);
			});

			_dataProcessor.Add("management", "commands", msg =>
			{
				ConsumeManagementPipeData<ManagementCommandInfo>(msg);
			});
		}

		private void ConsumePipeData<T>(string message) where  T : Info
		{
			var info = JsonConvert.DeserializeObject<T>(message);
			var envFolder = this.ApiInfoViewModels.FirstOrDefault(eF => eF.Name == info.Session);
			if (envFolder == null)
			{
				return;
			}

			envFolder.Add(info);
		}

		private void ConsumeManagementPipeData<T>(string message) where T : Info
		{
			var info = JsonConvert.DeserializeObject<T>(message);
			Console.WriteLine(info);
			this._dataRepository.AddManagementInfo(info);
		}

		private void SubscribeLogs()
		{
			Subscribe("log", msg =>
			{
				try
				{
					this.LogViewModel.Add(msg);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
					TraceLogger.Error($"Error in apiinfo subscribe method {e.Message} ");
				}
			});
		}

		private void SubscribeManagement()
		{
			Subscribe("management", msg =>
			{
				try
				{
					this.LogViewModel.Add(msg);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
					TraceLogger.Error($"Error in apiinfo subscribe method {e.Message} ");
				}
			});
		}

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
	}
}
