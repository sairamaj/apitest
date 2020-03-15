using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;
using System.Threading.Tasks;

namespace ApiManager.ViewModels
{
	class MainViewModel : CoreViewModel
	{
		public MainViewModel(IApiExecutor executor, IDataRepository dataRepository)
		{
			this.EnvironmentFolders = new SafeObservableCollection<CommandTreeViewModel>();

			foreach (var envInfo in dataRepository.GetEnvironments())
			{
				this.EnvironmentFolders.Add(new EnvironmentFolderViewModel(envInfo.Key, envInfo.Value, executor));
			}
		}

		public ObservableCollection<CommandTreeViewModel> EnvironmentFolders { get; set; }

	}
}
