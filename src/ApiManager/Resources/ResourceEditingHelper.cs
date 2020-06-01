using System.IO;
using System.Windows;
using ApiManager.Common.ViewModels;
using ApiManager.Common.Views;
using ApiManager.Resources.Model;

namespace ApiManager.Resources
{
	static class ResourceEditingHelper
	{
		public static ResourceData CreateNewResource(string method, string parentPath)
		{
			var win = new NewItemWindow();
			var vm = new NewItemWindowViewModel(win, $"New {method} item", name =>
			{
				var newFileName = Path.Combine(parentPath, name);
				if (File.Exists(newFileName))
				{
					MessageBox.Show($"{newFileName} exists already.");
					return false;
				}

				return true;
			});
			win.DataContext = vm;
			if (!win.ShowDialog().Value)
			{
				return null;
			}

			var fileName = Path.Combine(parentPath, vm.Name);
			File.WriteAllText(fileName, "{\r\n}");
			return new ResourceData(Path.Combine(parentPath, fileName));
		}

		public static ResourceData CreateNewResourceFolder(string method, string parentPath)
		{
			var win = new NewItemWindow();
			var vm = new NewItemWindowViewModel(win, $"New folder", name =>
			{
				var newDirectory = $"{parentPath}{Path.DirectorySeparatorChar}{name}";
				if (Directory.Exists(newDirectory))
				{
					MessageBox.Show($"{newDirectory} exists already.");
					return false;
				}

				return true;
			});

			win.DataContext = vm;
			if (!win.ShowDialog().Value)
			{
				return null;
			}

			var newResourceDirectory = $"{parentPath}{Path.DirectorySeparatorChar}{vm.Name}";
			if (!Directory.Exists(newResourceDirectory))
			{
				Directory.CreateDirectory(newResourceDirectory);
			}

			return new ResourceData(newResourceDirectory, true);
		}
	}
}
