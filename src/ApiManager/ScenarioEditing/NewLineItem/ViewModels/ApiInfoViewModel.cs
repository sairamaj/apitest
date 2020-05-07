using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class ApiInfoViewModel : CommandTreeViewModel
	{
		public ApiInfoViewModel(string rootCommand, string command, IEnumerable<string> subCommands) 
			: base(null, command, $"{rootCommand}_{command}")
		{
			RootCommand = rootCommand;
			Command = command;
			SubCommands = subCommands;
			this.IsExpanded = true;
		}

		public string RootCommand { get; }
		public string Command { get; }
		public IEnumerable<string> SubCommands { get; }

		protected override void LoadChildren()
		{
			this.SubCommands.ToList().ForEach(s =>
			this.Children.Add(new ApiInfoViewModel(this.Command, s, new List<string> { })));
		}

		public override object GetDragData()
		{
			if (this.RootCommand == null)
			{
				return null;		// only child routes can be dragged as they make full command.
			}

			var finalCommand = this.Command == "_" ? this.RootCommand : $"{this.RootCommand}.{this.Command}";
			var format = DataFormats.GetDataFormat("DragDropItemsControl");
			var dragViewModel = new ScenarioLineItemViewModel(
									new ApiScenarioItem(finalCommand), (a, e) => { });
			return new DataObject(format.Name, dragViewModel);
		}
	}
}