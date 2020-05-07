using System.Windows;
using ApiManager.Model;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	class BangCommandInfoViewModel : CommandTreeViewModel
	{
		public BangCommandInfoViewModel(BangCommand command) :base(null, command.Name, command.Name)
		{
			this.BangCommand = command;
			this.IsExpanded = true;
		}

		public BangCommand BangCommand { get; }
		public string Description => this.BangCommand.Description;

		public override object GetDragData()
		{
			var format = DataFormats.GetDataFormat("DragDropItemsControl");
			var dragViewModel = new ScenarioLineItemViewModel(
									new CommandScenarioItem(this.Name), (a, e) => { });
			return new DataObject(format.Name, dragViewModel);
		}
	}
}
