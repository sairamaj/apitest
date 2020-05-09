using System.Windows;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ViewModels;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class EditExtractCommandViewModel : DialogViewModel
	{
		public EditExtractCommandViewModel(
			Window win,
			CommandScenarioItem commandItem) : base(win)
		{
			this.CommandItem = commandItem;
		}

		public string JsonPath { get; set; }
		public string VariableName { get; set; }
		public string Command
		{
			get
			{
				return $"!extract {this.JsonPath} {this.VariableName}";
			}
		}

		public CommandScenarioItem CommandItem { get; }

		protected override bool OnClosing()
		{
			if (string.IsNullOrWhiteSpace(this.JsonPath))
			{
				MessageBox.Show($"Please enter json path.");
				return false;
			}
			else if (string.IsNullOrWhiteSpace(this.VariableName))
			{
				MessageBox.Show($"Please enter variable name.");
				return false;
			}

			this.CommandItem.Command = this.Command;
			return base.OnClosing();
		}
	}
}
