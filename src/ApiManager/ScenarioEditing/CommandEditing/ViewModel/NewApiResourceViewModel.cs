using System.IO;
using System.Windows;
using System.Windows.Input;
using ApiManager.Repository;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class NewApiResourceViewModel : CoreViewModel
	{
		public NewApiResourceViewModel(Window win, string method, string fileName, IResourceManager resourceManager)
		{
			this.Title = $"File for {method}";
			this.SaveCommand = new DelegateCommand(() =>
			{
				UiHelper.SafeAction(() =>
				{
					if (OnSave(method, resourceManager))
					{
						win.DialogResult = true;
						win.Close();
					}
				}
				, "Saving");
			});

			if (!string.IsNullOrWhiteSpace(fileName))
			{
				this.Name = Path.GetFileNameWithoutExtension(fileName);
				this.Content = File.ReadAllText(fileName);
			}
		}

		public string Title { get; set; }
		public string Name { get; set; }
		public ICommand SaveCommand { get; set; }
		public string Content { get; set; }

		private bool OnSave(string method, IResourceManager resourceManager)
		{
			if (string.IsNullOrWhiteSpace(this.Name))
			{
				MessageBox.Show("Please enter the name of the payload");
				return false;
			}

			var fileName = resourceManager.SaveApiRequestPayload(this.Content, this.Name, method);
			this.Name = Path.GetFileName(fileName);
			MessageBox.Show($"Saved {fileName}.");
			return true;
		}
	}
}
