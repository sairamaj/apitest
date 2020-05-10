using System.Windows;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ViewModels;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class EditPrintCommandViewModel : DialogViewModel
	{
		public EditPrintCommandViewModel(Window win, CommandScenarioItem commandItem) : base(win)
		{
			Message = $"{commandItem.Arg1} {commandItem.Arg2}";
			CommandItem = commandItem;
		}

		public string Message { get; set; }
		public CommandScenarioItem CommandItem { get; }

		protected override bool OnClosing()
		{
			if (string.IsNullOrWhiteSpace(this.Message))
			{
				MessageBox.Show($"Enter some message.");
				return false;
			}

			this.CommandItem.Command = $"!print {this.Message}";
			return base.OnClosing();
		}
	}
}
