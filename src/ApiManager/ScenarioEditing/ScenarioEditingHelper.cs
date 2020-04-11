using System.IO;
using ApiManager.Model;
using ApiManager.ScenarioEditing.ViewModel;
using ApiManager.ScenarioEditing.Views;

namespace ApiManager.ScenarioEditing
{
	class ScenarioEditingHelper
	{
		public static Scenario CreateNewScenarioContainer(string parentPath)
		{
			var win = new NewScenarioFolderWindow();
			var vm = new NewScenarioFolderEditViewModel(win, parentPath);
			win.DataContext = vm;
			var result = win.ShowDialog();
			if (!result.Value)
			{
				return null;
			}

			return new Scenario(Path.Combine(parentPath, vm.Name), true);
		}

		public static  Scenario CreateNewScenario(string parentPath)
		{
			var winx = new NewScenarioWindow();
			var vm = new NewScenarioEditViewModel(winx, parentPath);
			winx.DataContext = vm;
			var result = winx.ShowDialog();
			if (!result.Value)
			{
				return null;
			}

			return new Scenario(Path.Combine(parentPath, vm.Name) + ".txt");
		}
	}
}
