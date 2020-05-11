using System.Linq;
using System.Windows;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ViewModels;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class EditSetVariableCommandViewModel : DialogViewModel
	{
		public EditSetVariableCommandViewModel(
			Window win,
			CommandScenarioItem commandItem) : base(win)
		{
			this.CommandItem = commandItem;
			var parts = commandItem.Arg1.Split('=');
			this.VariableName = parts[0];
			if (parts.Length > 1)
			{
				this.VariableValue = string.Join("=", parts.Skip(1).ToArray());
			}
		}

		public string VariableName { get; set; }
		public string VariableValue { get; set; }

		public CommandScenarioItem CommandItem { get; }

		protected override bool OnClosing()
		{
			if (string.IsNullOrWhiteSpace(this.VariableName))
			{
				MessageBox.Show($"Please enter variable name.");
				return false;
			}
			else if (string.IsNullOrWhiteSpace(this.VariableValue))
			{
				MessageBox.Show($"Please enter variable value.");
				return false;
			}

			this.CommandItem.Command = $"!set {this.VariableName}={this.VariableValue}";
			return base.OnClosing();
		}
	}
}
