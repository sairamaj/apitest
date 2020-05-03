using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	class BangCommandInfoViewModel : CommandTreeViewModel
	{
		public BangCommandInfoViewModel(BangCommandInfo command) :base(null, command.Name, $"bang_command_{command.Name}")
		{
			Command = command;
			this.IsExpanded = true;
		}

		public BangCommandInfo Command { get; }
	}
}
