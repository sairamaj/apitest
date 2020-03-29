using System.Collections.Generic;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class CommandInfoViewModel : TreeViewItemViewModel
	{
		public CommandInfoViewModel(string mainCommand, IEnumerable<string> subCommands)
			: base(null, mainCommand, true)
		{
			this.SubCommands = subCommands;
			this.IsExpanded = true;
		}

		public IEnumerable<string> SubCommands { get; }

		protected override void LoadChildren()
		{
			foreach (var subCommand in this.SubCommands)
			{
				this.Children.Add(new SubCommandInfoViewModel(subCommand));
			}
		}
	}
}
