using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ApiManager.Repository;
using ApiManager.ScenarioEditing.CommandEditing.ViewModel;
using ApiManager.ScenarioEditing.CommandEditing.Views;
using ApiManager.ScenarioEditing.Models;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.ViewModel
{
	class ScenarioApiCommandLineItemViewModel : ScenarioLineItemViewModel
	{
		public ScenarioApiCommandLineItemViewModel(
			ApiScenarioItem lineItem,
			Action<ScenarioEditingAction, ScenarioLineItemViewModel> onEditAction):
			base(lineItem, onEditAction)
		{
			this.ApiScenarioItem = lineItem;
			this.EditPayloadFileCommand = new DelegateCommand(() => {
				UiHelper.SafeAction(() => this.EditResource(), "View");
			});
				
		}

		public ApiScenarioItem ApiScenarioItem { get; }

		public ICommand EditPayloadFileCommand { get; set; }

		private void EditResource()
		{
			var resourceManager = ServiceLocator.Locator.Resolve<IResourceManager>();
			var resourceWindow = new NewApiResourceWindow();
			var fileName = Path.Combine(
				resourceManager.GetResourcePath(this.ApiScenarioItem.Method), 
				this.ApiScenarioItem.PayloadFileName);
			var viewModel = new NewApiResourceViewModel(
				resourceWindow, 
				this.ApiScenarioItem.Method, 
				fileName,
				resourceManager);
			resourceWindow.DataContext = viewModel;
			resourceWindow.ShowDialog();
		}
	}
}
