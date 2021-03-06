﻿using System.Windows;
using ApiManager.Model;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	class FunctionCommandInfoViewModel : CommandTreeViewModel
	{
		public FunctionCommandInfoViewModel(FunctionCommand command) :base(null, command.Name, command.Name)
		{
			this.FunctionCommand = command;
			this.IsExpanded = true;
		}

		public FunctionCommand FunctionCommand { get; }
		public string Description => this.FunctionCommand.Description;

		public override object GetDragData()
		{
			var format = DataFormats.GetDataFormat("DragDropItemsControl");
			var functionName = $"__{this.Name}__";
			var dragViewModel = new ScenarioLineItemViewModel(
									new FunctionScenarioItem(functionName), (a, e) => { });
			return new DataObject(format.Name, dragViewModel);
		}
	}
}
