using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class ApiInfoViewModel: CommandTreeViewModel
	{
		public ApiInfoViewModel(string command, IEnumerable<string> subCommands):base(null, command, command)
		{
			SubCommands = subCommands;
			this.IsExpanded = true;
		}

		public IEnumerable<string> SubCommands { get; }

		protected override void LoadChildren()
		{
			this.SubCommands.ToList().ForEach(s => new ApiInfoViewModel(s, new List<string> { }));
		}

		public override object GetDragData()
		{
			var format = DataFormats.GetDataFormat("DragDropItemsControl");
			var dragViewModel = new ScenarioLineItemViewModel(
									new ApiScenarioItem(this.Name, new List<string> { }), (a, e) => { });
			return new DataObject(format.Name, dragViewModel);
		}
	}
}