using System;
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
					if (s.Session == null)
					{
						return;
					}
					var parts = s.Session.Split('=');
					if (parts.Length < 2)
					{
						return;
					}

					var envFolder = this.EnvironmentFolders.FirstOrDefault(eF => eF.Name == parts[0]);
					if (envFolder == null)
					{
						return;
					}

					envFolder.AddApiInfo(parts[1], s);
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
