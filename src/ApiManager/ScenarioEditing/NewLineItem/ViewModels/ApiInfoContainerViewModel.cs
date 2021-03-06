﻿using System.Collections.Generic;
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
			this.ApiInfoViewModels = apiCommandInfo.ApiCommands
				.OrderBy(c => c.Name)
				.Select(c => new ApiCommandViewModel(c));
			var list = new List<ApiRouteInfoViewModel>();
			this.IsExpanded = true;
		}

		public IEnumerable<ApiCommandViewModel> ApiInfoViewModels;
		protected override void LoadChildren()
		{
			foreach (var viewModel in this.ApiInfoViewModels)
			{
				this.Children.Add(viewModel);
			}
		}
	}
}
