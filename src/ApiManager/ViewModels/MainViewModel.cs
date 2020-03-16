using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Pipes;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class MainViewModel : CoreViewModel
	{
		private EnvironmentViewModel _selectedEnvironmentViewModel;
		private IApiExecutor apiExecutor;
		public MainViewModel(
			IApiExecutor executor,
			IDataRepository dataRepository,
			IMessageListener listener)
		{
			this.apiExecutor = executor ?? throw new ArgumentNullException(nameof(executor));
			this.Environments = new SafeObservableCollection<EnvironmentViewModel>();
			this.RunCommand = new DelegateCommand(async () => await this.RunAsync());
			foreach (var envInfo in dataRepository.GetEnvironments())
			{
				this.Environments.Add(new EnvironmentViewModel(envInfo, executor));
			}

			try
			{
				listener.SubScribe(s =>
				{
					var envFolder = this.Environments.FirstOrDefault(eF => eF.Name == s.Session);
					if (envFolder == null)
					{
						return;
					}

					envFolder.AddApiInfo(s);
				});
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}

			this.SelectedViewModel = this.Environments.FirstOrDefault();
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
				this.CommandFiles = this._selectedEnvironmentViewModel.EnvironmentInfo.CommandFiles;
				this.VariableFiles = this._selectedEnvironmentViewModel.EnvironmentInfo.VariableFiles;
				this.CurrentRequestResponseViewModel = new RequestResponseContainerViewModel(this._selectedEnvironmentViewModel.RequestResponses);
				OnPropertyChanged(() => this.CommandFiles);
				OnPropertyChanged(() => this.VariableFiles);
				OnPropertyChanged(() => this.CurrentRequestResponseViewModel);
			}
		}
		public string SelectedCommandFile { get; set; }
		public string SelectedVariableFile { get; set; }

		public IEnumerable<string> CommandFiles { get; set; }
		public IEnumerable<string> VariableFiles { get; set; }
		public ICommand RunCommand { get; set; }
		public RequestResponseContainerViewModel CurrentRequestResponseViewModel { get; set; }
		public async Task RunAsync()
		{
			if (this._selectedEnvironmentViewModel == null)
			{
				MessageBox.Show("Select Environment", "Environment", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (this.SelectedCommandFile == null)
			{
				MessageBox.Show("Select Command File", "Command File", MessageBoxButton.OK, MessageBoxImage.Error);
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
				var result = await this.apiExecutor.StartAsync(
					new TestData
					{
						ConfigName = Path.Combine(envInfo.Path,"config.json"),
						CommandsTextFileName = Path.Combine(envInfo.Path, this.SelectedCommandFile) + ".txt",
						VariablesFileName= Path.Combine(envInfo.Path, this.SelectedVariableFile) + ".var",
						SessionName = envInfo.Name,
					}
					);
			}
			catch (System.Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}
	}
}
