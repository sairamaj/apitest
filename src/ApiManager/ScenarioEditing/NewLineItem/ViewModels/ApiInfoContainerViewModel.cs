using System.Collections.Generic;
using System.Linq;
using ApiManager.Model;
using ApiManager.ViewModels;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class ApiInfoContainerViewModel : CommandTreeViewModel
	{
		public ApiInfoContainerViewModel(ApiCommandInfo apiCommandInfo) : base(null, "apis", "apis")
		{
			this.ApiInfoViewModels = apiCommandInfo.ApiCommands.Select(c => new ApiInfoViewModel(c.Name, c.Routes.Select(r => r.Name)));
			var list = new List<ApiInfoViewModel>();
			this.IsExpanded = true;
		}

		public IEnumerable<ApiInfoViewModel> ApiInfoViewModels;
		protected override void LoadChildren()
		{
			foreach (var viewModel in this.ApiInfoViewModels)
			{
				this.Children.Add(viewModel);
			}
		}
	}
}
