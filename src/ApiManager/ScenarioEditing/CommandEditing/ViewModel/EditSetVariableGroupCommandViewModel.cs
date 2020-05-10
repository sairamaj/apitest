using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Repository;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ViewModels;
using Wpf.Util.Core.Command;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class EditSetVariableGroupCommandViewModel : DialogViewModel
	{
		private readonly IResourceManager _resourceManager;
		private string _selectedFileName;

		public EditSetVariableGroupCommandViewModel(
			Window win,
			CommandScenarioItem commandItem,
			IResourceManager resourceManager) : base(win)
		{
			this.CommandItem = commandItem;
			this._resourceManager = resourceManager;
			this.SelectedFileName = string.IsNullOrEmpty(commandItem.Arg1) ? null : commandItem.Arg1;
			this.EditResourceCommand = new DelegateCommand(() =>
		   {
			   Process.Start("notepad.exe", Path.Combine(resourceManager.GetVariableGroupPath(), this.SelectedFileName));
		   });

			this.NewResourceCommand = new DelegateCommand(() =>
		   {
			   var p = Process.Start(new ProcessStartInfo
			   {
				   FileName = "notepad.exe",
				   WorkingDirectory = resourceManager.GetVariableGroupPath(),
			   });

			   p.Exited += (s, e) =>
			   {
				   this.Refresh();
			   };
		   });
		}

		public string SelectedFileName
		{
			get => this._selectedFileName;
			set
			{
				this._selectedFileName = value;
				OnPropertyChanged(() => this.SelectedFileName);
			}
		}

		public CommandScenarioItem CommandItem { get; }

		public IEnumerable<string> VariableGroupFileNames
		{
			get
			{
				return this._resourceManager.GetVariableGroupData()
					.OrderBy(v => v.FileWithExtension)
					.Select(v => v.FileWithExtension);
			}
		}

		public ICommand NewResourceCommand { get; set; }
		public ICommand EditResourceCommand { get; set; }
		protected override bool OnClosing()
		{
			if (string.IsNullOrWhiteSpace(SelectedFileName))
			{
				MessageBox.Show($"Please select variable group file name.");
				return false;
			}

			this.CommandItem.Command = $"!setgroup {this.SelectedFileName}";
			return base.OnClosing();
		}

		private new void Refresh()
		{
			OnPropertyChanged(() => this.VariableGroupFileNames);
		}
	}
}
