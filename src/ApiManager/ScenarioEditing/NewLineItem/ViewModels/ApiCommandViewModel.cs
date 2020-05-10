using System.Collections.Generic;
using System.Linq;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class ApiCommandViewModel : CommandTreeViewModel
	{
		public ApiCommandViewModel(ApiCommand apiCommand)
			: base(null, apiCommand.Name, $"{apiCommand.Name}_{apiCommand.Name}")
		{
			this.ApiCommand = apiCommand;
			this.IsExpanded = true;
		}

		public ApiCommand ApiCommand { get; }
		public string Description => this.ApiCommand.Description;
		protected override void LoadChildren()
		{
			if (this.ApiCommand.Routes == null)
			{
				return;
			}

			this.ApiCommand.Routes
				.OrderBy(r=> r.Name)
				.ToList().ForEach(r =>
			this.Children.Add(new ApiRouteInfoViewModel(this.ApiCommand, r)));
		}
	}
}