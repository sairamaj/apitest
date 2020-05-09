using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Utils;
using Wpf.Util.Core.Command;

namespace ApiManager.Resources.ViewModels
{
	class ViewResourceViewModel
	{
		public ViewResourceViewModel(string fileName)
		{
			this.Title = fileName;
			this.Content = File.ReadAllText(fileName);
			this.SaveCommand = new DelegateCommand(() =>
			{
				UiHelper.SafeAction(() =>
				{
					File.WriteAllText(fileName, Content);
					MessageBox.Show($"Saved {fileName}", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				, "Saving");
			});
		}

		public string Title { get; set; }
		public ICommand SaveCommand { get; set; }
		public string Content { get; set; }
	}
}
