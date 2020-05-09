using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ApiManager.Model;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class ApiRouteInfoViewModel : CommandTreeViewModel
	{
		public ApiCommand Command { get; }
		public ApiRoute Route { get; }

		public ApiRouteInfoViewModel(ApiCommand command, ApiRoute route) 
			: base(null, route.Name, $"{command.Name}_{route.Name}")
		{
			this.Command = command;
			this.Route = route;
			this.IsExpanded = true;
		}

		public string Description => this.Command.Description;

		protected override void LoadChildren()
		{
		}

		public override object GetDragData()
		{
			var finalCommand = this.Route.Name == "_" ? this.Command.Name : $"{this.Command.Name}.{this.Route.Name}";
			var format = DataFormats.GetDataFormat("DragDropItemsControl");
			var dragViewModel = new ScenarioApiCommandLineItemViewModel(
									new ApiScenarioItem(finalCommand), (a, e) => { });
			dragViewModel.IsDraggedAsNewItem = true;
			return new DataObject(format.Name, dragViewModel);
		}
	}
}