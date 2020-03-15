using System;
using System.Collections.ObjectModel;
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
			this.DataContext = this;
			this.RequestResponses = new SafeObservableCollection<ApiInfo>();

			this.RunCommand = new DelegateCommand(async () =>
			{
				try
				{
					var result = await executor.StartAsync(
						new TestData
						{
							ConfigName = env.Configuration,
							CommandsTextFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"TestFiles\{env.CommandFileName}"),
							VariablesFileName = env.VariableFileName
						}
						);
				}
				catch (System.Exception e)
				{
					MessageBox.Show(e.ToString());
				}
			});
		}

		public ICommand RunCommand { get; set; }
		public ObservableCollection<ApiInfo> RequestResponses { get; }

		public void AddApiInfo(ApiInfo apiInfo)
		{
			this.RequestResponses.Add(apiInfo);
		}
	}
}
