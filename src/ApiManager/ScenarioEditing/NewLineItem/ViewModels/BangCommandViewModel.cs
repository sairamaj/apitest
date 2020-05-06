using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ApiManager.Model;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class BangCommandViewModel : CommandTreeViewModel
	{
		public BangCommandViewModel(BangCommand command):base(null, command.Name, command.Name)
		{
			Command = command;
			this.IsExpanded = true;
		}

		public BangCommand Command { get; }

		public override object GetDragData()
		{
			//var format = DataFormats.GetDataFormat("DragDropItemsControl");
			//var dragViewModel = new ScenarioLineItemViewModel(
			//						new ApiScenarioItem(this.Name, new List<string> { }), (a, e) => { });
			//return new DataObject(format.Name, dragViewModel);
			return null;
		}
	}
}