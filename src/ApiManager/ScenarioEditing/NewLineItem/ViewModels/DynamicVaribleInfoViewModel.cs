using System.Windows;
using ApiManager.Model;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	class DynamicVaribleInfoViewModel : CommandTreeViewModel
	{
		public DynamicVaribleInfoViewModel(DynamicVariable variable) :base(null, variable.Name, variable.Name)
		{
			Variable = variable;
			this.IsExpanded = true;
		}

		public DynamicVariable Variable { get; }

		public override object GetDragData()
		{
			return null;
			//var format = DataFormats.GetDataFormat("DragDropItemsControl");
			//var dragViewModel = new ScenarioLineItemViewModel(
			//						new CommandScenarioItem(this.Name), (a, e) => { });
			//return new DataObject(format.Name, dragViewModel);
		}
	}
}
