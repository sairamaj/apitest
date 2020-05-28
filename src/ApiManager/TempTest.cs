using System.Collections.Generic;
using System.Windows;
using ApiManager.ApiConfigEditing.ViewModels;
using ApiManager.ApiConfigEditing.Views;
using ApiManager.Model;
using ApiManager.Repository;

namespace ApiManager
{
	class TempTest
	{
		private readonly IApiSpecRepository repository;
		private readonly IDataRepository dataRepository;

		public TempTest(IApiSpecRepository repository, IDataRepository dataRepository)
		{
			this.repository = repository;
			this.dataRepository = dataRepository;
		}

		public void AddRouteTest()
		{
			AddRouteWindow win = new AddRouteWindow();
			var vm = new AddRouteWindowViewModel(win);
			win.DataContext = vm;
			win.ShowDialog();
			MessageBox.Show(vm.Name);
			MessageBox.Show(vm.Path);
			System.Environment.Exit(-1);
		}

		public void TestConfigEditing()
		{
			var doc = this.repository.GetFromOpenApiSpecFile(@"c:\temp\petstore.yaml");
			var apiCmdInfo = new ApiCommandInfo();
			apiCmdInfo.ApiCommands = new List<ApiCommand>
			{
				new ApiCommand{
					Name = "api",
					Description = "api description here",
					Routes = new List<ApiRoute>{
						new ApiRoute{
							Name = "_",
							Path = "/",
							Description = "api root"
						},
						new ApiRoute{
							Name = "foo",
							Path = "/bar",
							Description = "api foo help"
						}
					}
				}
			};
			var vm = new EditorViewModel(doc, apiCmdInfo);
			var win = new EditorWindow { DataContext = vm };
			win.ShowDialog();
			System.Environment.Exit(-1);
		}
	}
}
