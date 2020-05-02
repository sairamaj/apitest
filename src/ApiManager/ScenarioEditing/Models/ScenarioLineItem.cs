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
			OriginalLine = originalLine;
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

			this.MoveUpCommand = new DelegateCommand(() => {
				MessageBox.Show("Move up!!!");
			});
			this.MoveDownCommand = new DelegateCommand(() => {
				MessageBox.Show("Move delete!!!");
			});
			this.DeleteCommand = new DelegateCommand(() => {
				MessageBox.Show("delete!!!");
			});
		}

		public string OriginalLine { get; }
		public string Type { get; set; }
		public ICommand StartEditingModeCommand { get; }
		public ICommand MoveUpCommand { get; }
		public ICommand MoveDownCommand { get; }
		public ICommand DeleteCommand { get; }

		public bool EditingModeOn { get; set; }
		public ICommand DoneWithEditingCommand { get; }
		public abstract string GetCommand();
	}
}
