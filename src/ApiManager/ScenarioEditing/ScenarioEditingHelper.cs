using System.IO;
using System.Windows;
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

		public static Scenario CreateNewScenario(string parentPath)
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

		public static Scenario CopyScenario(Scenario scenario)
		{
			var win = new NewScenarioWindow();
			var vm = new NewScenarioEditViewModel(win, scenario.ContainerPath);
			win.DataContext = vm;
			var result = win.ShowDialog();
			if (!result.Value)
			{
				return null;
			}

			var newFileName = Path.Combine(scenario.ContainerPath, vm.Name) + ".txt";
			File.WriteAllText(newFileName, File.ReadAllText(scenario.FileName));
			return new Scenario(newFileName);
		}

		public static bool DeleteScenario(Scenario scenario)
		{
			var msg = string.Empty;
			if (scenario.IsContainer)
			{
				msg = $"Are u sure you want to delete Folder: {scenario.Name}. " +
					$"All Child scenarios will be deleted recursively.";
			}
			else
			{
				msg = $"Are u sure you want to delete {scenario.Name}";
			}
			var result = MessageBox.Show(
						 msg,
						 "Confirm",
						 MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.No)
			{
				return false;
			}

			if (scenario.IsContainer)
			{
				Directory.Delete(scenario.ContainerPath, true);
			}
			else
			{
				if (File.Exists(scenario.FileName))
				{
					File.Delete(scenario.FileName);
				}
			}
			return true;
		}
	}
}
