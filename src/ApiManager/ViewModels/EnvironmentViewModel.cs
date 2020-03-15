using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public EnvironmentViewModel(string name, IApiExecutor executor) : base(null, name, name)
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
							ConfigName = "apigee.json",
							CommandsTextFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestFiles\list_apis.txt"),
							VariablesFileName = Path.Combine(@"c:\temp\test.var")
						}
						);
					MessageBox.Show(result);
				}
				catch (System.Exception e)
				{
					MessageBox.Show(e.ToString());
				}
			});

			this.RequestResponses.Add(new ApiInfo { Method = "GET" , Request = new Request { Body = "body1 here"}, Response = new Response {Content = "content1 here " } });
			this.RequestResponses.Add(new ApiInfo { Method = "POST", Request = new Request { Body = "body2 here" }, Response = new Response { Content = "content2 here " } });

		}

		public ICommand RunCommand { get; set; }
		public ObservableCollection<ApiInfo> RequestResponses { get; }

	}
}
