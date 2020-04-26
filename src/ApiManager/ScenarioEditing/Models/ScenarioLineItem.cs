using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.Models
{
	class ScenarioLineItem : CoreViewModel
	{
		public ScenarioLineItem(string type)
		{
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

		public string Type { get; set; }
		public ICommand StartEditingModeCommand { get; }
		public bool EditingModeOn { get; set; }
		public ICommand DoneWithEditingCommand { get; }
	}
}
