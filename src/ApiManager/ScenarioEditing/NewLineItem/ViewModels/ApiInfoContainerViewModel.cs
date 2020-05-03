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
			var list = new List<ApiInfoViewModel>();
			foreach (var cmd in apiCommandInfo.ApiCommands)
			{
				list.Add(new ApiInfoViewModel(cmd.Key, cmd.Value));
			}

			this.ApiInfoViewModels = list;
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
