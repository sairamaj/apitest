using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ApiConfigEditing.ViewModels
{
	class ApiRouteInfoViewModel : CommandTreeViewModel
	{
		public ApiRouteInfoViewModel(ApiCommand command, ApiRoute route)
			: base(null, route.Name, $"{command.Name}_{route.Name}")
		{
			this.Command = command;
			this.Route = route;
			this.IsExpanded = true;
			this.AddApiRouteCommand = new DelegateCommand(() =>
		   {
			   MessageBox.Show("add here.");
		   });
		}

		public ApiCommand Command { get; }
		public ApiRoute Route { get; }
		public ICommand AddApiRouteCommand { get; private set; }

		public string Description => this.Command.Description;

		protected override void LoadChildren()
		{
		}
	}
}
