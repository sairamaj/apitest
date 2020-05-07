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
		protected override void LoadChildren()
		{
			if (this.ApiCommand.Routes == null)
			{
				return;
			}

			this.ApiCommand.Routes.ToList().ForEach(r =>
			this.Children.Add(new ApiRouteInfoViewModel(this.ApiCommand, r)));
		}
	}
}