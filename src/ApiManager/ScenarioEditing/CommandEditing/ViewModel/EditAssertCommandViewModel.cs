using System;
using System.Linq;
using System.Windows;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ViewModels;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class EditAssertCommandViewModel : DialogViewModel
	{
		const string StatusCodeDisplayName = "Status Code";
		const string ResponseValueDisplayName = "Response Value";
		private string _selectedSource;

		public EditAssertCommandViewModel(
			Window win,
			CommandScenarioItem commandItem) : base(win)
		{
			this.SelectedSource = this.Sources.First();
			this.StatusCode = string.IsNullOrEmpty(commandItem.Arg2) ? 200: Convert.ToInt32(commandItem.Arg2);
			this.JsonPath = commandItem.Arg1;
			this.JsonValue = commandItem.Arg2;
			this.CommandItem = commandItem;
		}

		public string[] Sources => new string[] { StatusCodeDisplayName, ResponseValueDisplayName };	// if you change this , change . xaml also
		public int StatusCode { get; set; }
		public string JsonPath { get; set; }
		public string JsonValue { get; set; }
		public string SelectedSource
		{
			get => this._selectedSource;
			set
			{
				this._selectedSource = value;
				OnPropertyChanged(() => this.SelectedSource);
			}
		}

		public string Command
		{
			get
			{
				var cmd = "!assert";
				if (this.SelectedSource == StatusCodeDisplayName)
				{
					cmd += $" status_code {this.StatusCode}";
				}
				else
				{
					cmd += $" {this.JsonPath} {this.JsonValue}";
				}

				return cmd;
			}
		}

		public CommandScenarioItem CommandItem { get; }

		protected override bool OnClosing()
		{
			if (this.SelectedSource == StatusCodeDisplayName )
			{
				if (this.StatusCode == 0)
				{
					MessageBox.Show($"Please enter valid status code.");
					return false;
				}
			}
			else if (string.IsNullOrWhiteSpace(this.JsonPath))
			{
				MessageBox.Show($"Please enter json path.");
				return false;
			}

			this.CommandItem.Command = this.Command;
			return base.OnClosing();
		}
		
	}
}
