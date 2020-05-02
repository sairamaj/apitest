using System.Collections.Generic;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class MainViewModel : CoreViewModel
	{
		public MainViewModel(
			IEnumerable<BangCommandInfo> bangCommands,
			ApiCommandInfo apiCommandInfo)
		{
			this.RootCommands = new List<CommandTreeViewModel>()
			{
				new ApiInfoContainerViewModel(apiCommandInfo),
				new BangContainerCommandInfoViewModel(bangCommands),
				new FunctionInfoViewModel("functions", "functions")
			};
		}

		public IEnumerable<CommandTreeViewModel> RootCommands { get; }
	}
}
