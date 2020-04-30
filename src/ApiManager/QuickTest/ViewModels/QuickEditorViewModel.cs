using System.Collections.Generic;
using System.Linq;
using ApiManager.Model;
using ApiManager.ViewModels;

namespace ApiManager.QuickTest.ViewModels
{
	class QuickEditorViewModel
	{
		public QuickEditorViewModel(ApiCommandInfo commandInfo, Environment environment)
		{
			this.Commands = commandInfo.ApiCommands
				.OrderBy(c => c.Key)
				.Select(c => new CommandInfoViewModel(c.Key, c.Value));
			this.EditorViewModel = new HttpRequestEditorViewModel(environment);
		}

		public IEnumerable<CommandInfoViewModel> Commands { get; set; }
		public HttpRequestEditorViewModel EditorViewModel { get; private set; }
	}
}
