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
			var name = GetResourceNme(method, parentPath);
			if (name == null)
			{
				return null;
			}

			var fileName = Path.Combine(parentPath, name);
			if (Path.GetExtension(fileName).Length == 0)
			{
				fileName = $"{fileName}.json";
			}
			File.WriteAllText(fileName, "{\r\n}");
			return new ResourceData(Path.Combine(parentPath, fileName));
		}

		public static ResourceData CreateNewResourceFolder(string method, string parentPath)
		{
			var win = new NewItemWindow();
			var vm = new NewItemWindowViewModel(win, $"New {method} resource folder", name =>
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

		public static ResourceData CopyResource(string method, ResourceData resource)
		{
			var name = GetResourceNme(method, resource.ContainerPath);
			if (name == null)
			{
				return null;
			}

			var newFileName = Path.Combine(resource.ContainerPath, name);
			File.WriteAllText(newFileName, resource.GetData());
			return new ResourceData(newFileName);
		}

		public static bool DeleteResource(ResourceData resource)
		{
			var msg = string.Empty;
			if (resource.IsContainer)
			{
				msg = $"Are u sure you want to delete Folder: {resource.Name}. " +
					$"All Child resources will be deleted recursively.";
			}
			else
			{
				msg = $"Are u sure you want to delete {resource.Name}";
			}
			var result = MessageBox.Show(
						 msg,
						 "Confirm",
						 MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.No)
			{
				return false;
			}

			if (resource.IsContainer)
			{
				if (Directory.Exists(resource.ContainerPath))
				{
					Directory.Delete(resource.ContainerPath, true);
				}
			}
			else
			{
				if (File.Exists(resource.FileName))
				{
					File.Delete(resource.FileName);
				}
			}

			return true;
		}

		private static string GetResourceNme(string method, string parentPath)
		{
			var win = new NewItemWindow();
			var vm = new NewItemWindowViewModel(win, $"New {method} item", name =>
			{
				if (Path.GetExtension(name).Length == 0)
				{
					name = $"{name}.json";
				}

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

			return vm.Name;
		}
	}
}
