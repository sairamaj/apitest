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
using ApiManager.Pipes;
using System.Linq;

namespace ApiManager.ViewModels
{
	class MainViewModel : CoreViewModel
	{
		public MainViewModel(
			IApiExecutor executor, 
			IDataRepository dataRepository,
			IMessageListener listener)
		{
			this.EnvironmentFolders = new SafeObservableCollection<EnvironmentFolderViewModel>();

			foreach (var envInfo in dataRepository.GetEnvironments())
			{
				this.EnvironmentFolders.Add(new EnvironmentFolderViewModel(envInfo.Key, envInfo.Value, executor));
			}

			try
			{
				listener.SubScribe(s =>
				{
					var envFolder = this.EnvironmentFolders.FirstOrDefault();
					envFolder.AddApiInfo("", s);
				});
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		public ObservableCollection<EnvironmentFolderViewModel> EnvironmentFolders { get; set; }

	}
}
