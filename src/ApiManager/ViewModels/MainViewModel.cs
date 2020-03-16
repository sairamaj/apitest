using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ApiManager.Pipes;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class MainViewModel : CoreViewModel
	{
		private EnvironmentViewModel _selectedEnvironmentViewModel;
		public MainViewModel(
			IApiExecutor executor,
			IDataRepository dataRepository,
			IMessageListener listener)
		{
			this.Environments = new SafeObservableCollection<EnvironmentViewModel>();
			foreach (var envInfo in dataRepository.GetEnvironments())
			{
				this.Environments.Add(new EnvironmentViewModel(envInfo, executor));
			}

			try
			{
				listener.SubScribe(s =>
				{
					if (s.Session == null)
					{
						return;
					}
					var parts = s.Session.Split('=');
					if (parts.Length < 2)
					{
						return;
					}

					var envFolder = this.Environments.FirstOrDefault(eF => eF.Name == parts[0]);
					if (envFolder == null)
					{
						return;
					}

					// envFolder.AddApiInfo(parts[1], s);
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
				OnPropertyChanged(() => this.CommandFiles);
				OnPropertyChanged(() => this.VariableFiles);
			}
		}

		public IEnumerable<string> CommandFiles { get; set; }
		public IEnumerable<string> VariableFiles { get; set; }
	}
}
