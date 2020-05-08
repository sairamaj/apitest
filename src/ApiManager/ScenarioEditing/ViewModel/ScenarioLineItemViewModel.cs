using System;
using System.Windows.Input;
using ApiManager.ScenarioEditing.Models;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.ViewModel
{
	class ScenarioLineItemViewModel : CoreViewModel
	{
		public ScenarioLineItemViewModel(
			ScenarioLineItem lineItem,
			Action<ScenarioEditingAction, ScenarioLineItemViewModel> onEditAction)
		{
			this.LineItem = lineItem ?? throw new System.ArgumentNullException(nameof(lineItem));
			this.MoveUpCommand = new DelegateCommand(() =>
			{
				onEditAction(ScenarioEditingAction.MoveUp, this);
			});
			this.MoveDownCommand = new DelegateCommand(() =>
			{
				onEditAction(ScenarioEditingAction.MoveDown, this);
			});
			this.DeleteCommand = new DelegateCommand(() =>
			{
				onEditAction(ScenarioEditingAction.Delete, this);
			});
			this.CommentCommand = new DelegateCommand(() =>
			{
				this.LineItem.ToggleComment();
			});

		}
		public bool IsDraggedAsNewItem { get; set; }
		public ScenarioLineItem LineItem { get; }
		public ICommand DeleteCommand { get; }
		public ICommand CommentCommand { get; }
		public ICommand MoveUpCommand { get; }
		public ICommand MoveDownCommand { get; }
		
	}
}
