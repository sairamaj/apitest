using System;
using System.Diagnostics;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.ScenarioEditing;
using ApiManager.ScenarioEditing.ViewModel;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ScenarioBaseViewModel: CommandTreeViewModel
	{
		protected Action<ScenarioAction, Scenario> _onEvent;

		public ScenarioBaseViewModel(
			TreeViewItemViewModel parent, 
			Scenario scenario,
			Action<ScenarioAction, Scenario> onEvent)
			: base(parent, scenario.Name, scenario.FileName)
		{
			this.Scenario = scenario;
			this._onEvent = onEvent;

			this.RelvealInExplorerCommand = new DelegateCommand(() =>
			{
				Process.Start(this.Scenario.ContainerPath);
			});
			this.DeleteCommand = new DelegateCommand(() => this.DeleteScenario());

		}

		public Scenario Scenario { get; }
		public ICommand RelvealInExplorerCommand { get; }
		public ICommand DeleteCommand { get; }

		private void DeleteScenario()
		{
			var deletedScenario = ScenarioEditingHelper.DeleteScenario(this.Scenario);
			if (!deletedScenario)
			{
				return;
			}
			this._onEvent(ScenarioAction.Delete, this.Scenario);
		}
	}
}
