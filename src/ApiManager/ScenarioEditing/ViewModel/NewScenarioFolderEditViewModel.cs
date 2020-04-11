﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.ViewModel
{
	class NewScenarioFolderEditViewModel : CoreViewModel
	{
		public NewScenarioFolderEditViewModel(Window win, string containerName)
		{
			ContainerName = containerName;
			this.OkCommand = new DelegateCommand(() =>
			{
				if (CreateScenarioFolder())
				{
					win.DialogResult = true;
					win.Close();
				}
			});
			this.CancelCommand = new DelegateCommand(() =>
			{
				win.DialogResult = false;
				win.Close();

			});
		}

		private bool CreateScenarioFolder()
		{
			if (string.IsNullOrWhiteSpace(this.Name))
			{
				MessageBox.Show("Enter name of the scenario container", "Name required", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (this.Name.Contains("."))
			{
				MessageBox.Show($"{this.Name} cannot contain periods", "Invalid folder name", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			// Try creating the file
			var directoryName = Path.Combine(this.ContainerName, this.Name);
			if (Directory.Exists(directoryName))
			{
				MessageBox.Show($"{this.Name} already exists", "Duplicate scenario container", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			Directory.CreateDirectory(directoryName);
			return true;
		}

		public ICommand OkCommand { get; }
		public ICommand CancelCommand { get; }
		public string Name { get; set; }
		public string ContainerName { get; }
	}
}
