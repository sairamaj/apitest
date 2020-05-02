using System;
using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.Models
{
	internal abstract class ScenarioLineItem : CoreViewModel
	{
		public ScenarioLineItem(string type, string originalLine)
		{
			this.OriginalLine = originalLine;
			Type = type;
			this.StartEditingModeCommand = new DelegateCommand(() =>
			{
				this.EditingModeOn = true;
				OnPropertyChanged(() => this.EditingModeOn);
			});
			this.DoneWithEditingCommand = new DelegateCommand(() =>
		   {
			   this.EditingModeOn = false;
			   OnPropertyChanged(() => this.EditingModeOn);
		   });
		}

		public string OriginalLine { get; private set; }
		public string Type { get; set; }
		public ICommand StartEditingModeCommand { get; }
		public bool EditingModeOn { get; set; }
		public ICommand DoneWithEditingCommand { get; }
		public abstract string GetCommand();
		public abstract void ToggleComment();
		public bool IsCommented { get; set; }

	}
}
