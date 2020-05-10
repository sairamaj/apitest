using System;
using System.Windows;
using System.Windows.Input;
using ApiManager.ScenarioEditing.Models;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.ViewModel
{
	class ScenarioLineItemViewModel : CoreViewModel
	{
		Action<ScenarioEditingAction, ScenarioLineItemViewModel> _onEditAction;
		public ScenarioLineItemViewModel(
			ScenarioLineItem lineItem,
			Action<ScenarioEditingAction, ScenarioLineItemViewModel> onEditAction)
		{
			this._onEditAction = onEditAction;
			this.LineItem = lineItem ?? throw new System.ArgumentNullException(nameof(lineItem));
			this.DeleteCommand = new DelegateCommand(() =>
			{
				this._onEditAction(ScenarioEditingAction.Delete, this);
			});
			this.CommentCommand = new DelegateCommand(() =>
			{
				this.LineItem.ToggleComment();
			});
			this.EditScenarioLineItemCommand = new DelegateCommand(() => {
				this._onEditAction(ScenarioEditingAction.Edit, this);
			});
		}
		public bool IsDraggedAsNewItem { get; set; }
		public ScenarioLineItem LineItem { get; }
		public ICommand DeleteCommand { get; }
		public ICommand CommentCommand { get; }
		public ICommand EditScenarioLineItemCommand { get; }
		public ICommand MoveUpCommand { get; }
		public ICommand MoveDownCommand { get; }
		public void AttachEditAction(Action<ScenarioEditingAction, ScenarioLineItemViewModel> onEditAction)
		{
			this._onEditAction = onEditAction;
		}
	}
}
