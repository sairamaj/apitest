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

namespace ApiManager.ViewModels
{
	class MainViewModel : CoreViewModel
	{
		public MainViewModel(IApiExecutor exeutor)
		{
			this.EnvironmentFolders = new SafeObservableCollection<CommandTreeViewModel>();
			this.EnvironmentFolders.Add(new EnvironmentFolderViewModel("Azure"));
			this.EnvironmentFolders.Add(new EnvironmentFolderViewModel("Apigee"));
			this.TestCommand = new DelegateCommand(async () =>
			{
				try
				{
					var env = new EnvironmentInfo
					{
						Name = "Apigee-Sairama",
						ConfigName = "apigee.json"
					};
					var result = await exeutor.StartAsync(
						env, 
						Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestFiles\list_apis.txt"));
					MessageBox.Show(result);
				}
				catch (System.Exception e)
				{
					MessageBox.Show(e.ToString());
				}
			});
		}

		public ObservableCollection<CommandTreeViewModel> EnvironmentFolders { get; set; }
		public ICommand TestCommand { get; set; }
	}
}
