using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class EnvironmentViewModel : CommandTreeViewModel
	{
		public EnvironmentViewModel(EnvironmentInfo env, IApiExecutor executor) : base(null, env.Name, env.Name)
		{
			this.IsExpanded = true;
			this.EnvironmentInfo = env;
			this.DataContext = this;
			this.RequestResponses = new SafeObservableCollection<ApiInfoViewModel>();
			this.EditConfigFileCommand = new DelegateCommand(() =>
			{
				try
				{
					Process.Start("notepad", this.EnvironmentInfo.Configuration);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			});

		}

		public EnvironmentInfo EnvironmentInfo { get; set; }

		public ICommand RunCommand { get; set; }
		public ObservableCollection<ApiInfoViewModel> RequestResponses { get; }

		public void AddApiInfo(ApiInfo apiInfo)
		{
			this.RequestResponses.Add(new ApiInfoViewModel(apiInfo));
		}
		public ICommand EditConfigFileCommand { get; }

	}
}
