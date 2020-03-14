using System.Collections.ObjectModel;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class MainViewModel : CoreViewModel
	{
		public MainViewModel()
		{
			this.EnvironmentFolders = new SafeObservableCollection<CommandTreeViewModel>();
			this.EnvironmentFolders.Add(new EnvironmentFolderViewModel("Azure"));
			this.EnvironmentFolders.Add(new EnvironmentFolderViewModel("Apigee"));
		}

		public ObservableCollection<CommandTreeViewModel> EnvironmentFolders { get; set; }
	}
}
