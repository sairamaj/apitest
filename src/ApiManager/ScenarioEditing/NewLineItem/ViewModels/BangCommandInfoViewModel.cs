using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	class BangCommandInfoViewModel : CommandTreeViewModel
	{
		public BangCommandInfoViewModel(BangCommand command) :base(null, command.Name, command.Name)
		{
			Command = command;
			this.IsExpanded = true;
		}

		public BangCommand Command { get; }
	}
}
