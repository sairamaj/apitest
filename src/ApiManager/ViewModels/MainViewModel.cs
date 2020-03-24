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
		private EnvironmentViewModel _selectedEnvironmentViewModel;
		private IMessageListener _listener;
		private IApiExecutor _apiExecutor;
		public MainViewModel(
			IApiExecutor executor,
			IDataRepository dataRepository,
			IMessageListener listener)
		{
			this._apiExecutor = executor ?? throw new ArgumentNullException(nameof(executor));
			this._listener = listener ?? throw new ArgumentNullException(nameof(listener));

			this.Environments = new SafeObservableCollection<EnvironmentViewModel>();
			this.RunCommand = new DelegateCommand(async () => await this.RunAsync());
			this.OpenCommandPrompt = new DelegateCommand(async () => await this.OpenCommandPromptAsync());
			this.ShowIssuesCommand = new DelegateCommand(() =>
			{
				try
				{
					Process.Start("issues.txt");
				}
				catch (Exception e)
				{
					MessageBox.Show($"{e.Message} - issues.txt", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			});
			foreach (var envInfo in dataRepository.GetEnvironments())
			{
				this.Environments.Add(new EnvironmentViewModel(envInfo, executor));
			}

			try
			{
				SubscribeApiInfo();
				SubscribeLogs();
				// SubscribeManagement();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}

			this.SelectedViewModel = this.Environments.FirstOrDefault();
			this.LogViewModel = new LogViewModel();
			//this.CommandFiles = this.SelectedViewModel.EnvironmentInfo.CommandFiles.Select(c => new CommandFileViewModel(c));
			//this.VariableFiles = this.SelectedViewModel.EnvironmentInfo.VariableFiles.Select(v => new VariableFileViewModel(v));
			//this.SelectedCommandFile = this.CommandFiles.FirstOrDefault();
			//this.SelectedVariableFile = this.VariableFiles.FirstOrDefault();
		}

		public ObservableCollection<EnvironmentViewModel> Environments { get; set; }

		public EnvironmentViewModel SelectedViewModel
		{
			get
			{
				return _selectedEnvironmentViewModel;
			}
			set
			{
				this._selectedEnvironmentViewModel = value;
				//this.CommandFiles = this._selectedEnvironmentViewModel.EnvironmentInfo.CommandFiles.Select(c => new CommandFileViewModel(c));
				//this.VariableFiles = this._selectedEnvironmentViewModel.EnvironmentInfo.VariableFiles.Select(v => new VariableFileViewModel(v));

				this.CurrentRequestResponseViewModel = new RequestResponseContainerViewModel(this._selectedEnvironmentViewModel.RequestResponses);
				//OnPropertyChanged(() => this.CommandFiles);
				//OnPropertyChanged(() => this.VariableFiles);
				OnPropertyChanged(() => this.CurrentRequestResponseViewModel);
				var task = ChangeAsync();
			}
		}
		public CommandFileViewModel SelectedCommandFile { get; set; }
		public VariableFileViewModel SelectedVariableFile { get; set; }

		public IEnumerable<CommandFileViewModel> CommandFiles { get; set; }
		public IEnumerable<VariableFileViewModel> VariableFiles { get; set; }
		public ICommand RunCommand { get; set; }
		public ICommand OpenCommandPrompt { get; set; }
		public ICommand ShowIssuesCommand { get; set; }
		public RequestResponseContainerViewModel CurrentRequestResponseViewModel { get; set; }

		public LogViewModel LogViewModel { get; set; }
		public async Task RunAsync()
		{
			if (this._selectedEnvironmentViewModel == null)
			{
				MessageBox.Show("Select Environment", "Environment", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedCommandFile == null)
			{
				MessageBox.Show("Select Scenario File", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedVariableFile == null)
			{
				MessageBox.Show("Select Variable File", "Variable File", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				var envInfo = this.SelectedViewModel.EnvironmentInfo;
				var result = await _apiExecutor.StartAsync(
					new TestData
					{
						ConfigName = envInfo.Configuration,
						CommandsTextFileName = SelectedCommandFile.FileName,
						VariablesFileName = SelectedVariableFile.FileName,
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
			if (this._selectedEnvironmentViewModel == null)
			{
				MessageBox.Show("Select Environment", "Environment", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedCommandFile == null)
			{
				MessageBox.Show("Select Scenario File", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedVariableFile == null)
			{
				MessageBox.Show("Select Variable File", "Variable File", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try
			{
				var envInfo = this.SelectedViewModel.EnvironmentInfo;
				var result = await _apiExecutor.OpenCommandPromptAsync(
					new TestData
					{
						ConfigName = envInfo.Configuration,
						CommandsTextFileName = SelectedCommandFile.FileName,
						VariablesFileName = SelectedVariableFile.FileName,
						SessionName = envInfo.Name,
					}
					).ConfigureAwait(false);
			}
			catch (System.Exception e)
			{
				MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private async Task ChangeAsync()
		{
			await Task.Delay(0);
			this.CommandFiles = this._selectedEnvironmentViewModel.EnvironmentInfo.CommandFiles.Select(c => new CommandFileViewModel(c));
			this.VariableFiles = this._selectedEnvironmentViewModel.EnvironmentInfo.VariableFiles.Select(v => new VariableFileViewModel(v));
			OnPropertyChanged(() => this.CommandFiles);
			OnPropertyChanged(() => this.VariableFiles);

			this.SelectedCommandFile = this.CommandFiles.FirstOrDefault();
			this.SelectedVariableFile = this.VariableFiles.FirstOrDefault();
			OnPropertyChanged(() => this.SelectedCommandFile);
			OnPropertyChanged(() => this.SelectedVariableFile);
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
			Subscribe("apiinfo", msg =>
			{
				try
				{
					if (!msg.StartsWith("api|"))
					{
						var partialMessage = msg.Substring(0, msg.Length > 50 ? 50 : msg.Length);
						this.LogViewModel.Add($"[Warning] does not start with api| {partialMessage}");
						return;
					}

					msg = msg.Substring("api|".Length);
					var apiInfo = JsonConvert.DeserializeObject<ApiInfo>(msg);
					var envFolder = this.Environments.FirstOrDefault(eF => eF.Name == apiInfo.Session);
					if (envFolder == null)
					{
						return;
					}

					envFolder.AddApiInfo(apiInfo);
				}
				catch (Exception e)
				{
					var partialMessage = msg.Substring(0, msg.Length > 50 ? 50 : msg.Length);
					TraceLogger.Error($"Error in apiinfo subscribe method {e.Message} ");
					this.LogViewModel.Add($"[Error] in deserializing {partialMessage} {e.Message}");
				}
			});
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
	}
}
