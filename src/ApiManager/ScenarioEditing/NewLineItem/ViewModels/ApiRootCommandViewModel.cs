using System.Collections.Generic;
using System.Linq;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class ApiRootCommandViewModel : CommandTreeViewModel
	{
		public ApiRootCommandViewModel(string rootCommand, string command, IEnumerable<string> subCommands) 
			: base(null, command, $"{rootCommand}_{command}")
		{
			RootCommand = rootCommand;
			Command = command;
			SubCommands = subCommands;
			this.IsExpanded = true;
		}

		public string RootCommand { get; }
		public string Command { get; }
		public IEnumerable<string> SubCommands { get; }

		protected override void LoadChildren()
		{
			this.SubCommands.ToList().ForEach(s =>
			this.Children.Add(new ApiInfoViewModel(this.Command, s, new List<string> { })));
		}
	}
}