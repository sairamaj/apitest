using System.IO;
using System.Windows;
using System.Windows.Input;
using ApiManager.Repository;
using ApiManager.Resources.ViewModels;
using ApiManager.Resources.Views;
using ApiManager.ScenarioEditing.Models;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ScenarioResourceViewModel : CoreViewModel
	{
		public ScenarioResourceViewModel(ApiScenarioItem apiScenarioItem)
		{
			Name = apiScenarioItem.PayloadFileName;
			this.ResourceCommand = new DelegateCommand(() =>
			{
				var fileName = Path.Combine(
				ServiceLocator.Locator.Resolve<IResourceManager>().GetResourcePath(apiScenarioItem.Method),
				apiScenarioItem.PayloadFileName);
				UiHelper.SafeAction(() =>
				{
					var win = new ViewResourceWindow
					{
						DataContext = new ViewResourceViewModel(fileName)
					};
					win.ShowDialog();
				}, "View");
			});
		}

		public string Name { get; }
		public ICommand ResourceCommand { get; }

	}
}
