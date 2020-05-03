using System.Collections.Generic;
using System.Linq;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class ApiInfoViewModel: CommandTreeViewModel
	{
		public ApiInfoViewModel(string command, IEnumerable<string> subCommands):base(null, command, command)
		{
			SubCommands = subCommands;
			this.IsExpanded = true;
		}

		public IEnumerable<string> SubCommands { get; }

		protected override void LoadChildren()
		{
			this.SubCommands.ToList().ForEach(s => new ApiInfoViewModel(s, new List<string> { }));
		}

		public override object GetDragData()
		{
			return this.Name;
		}
	}
}