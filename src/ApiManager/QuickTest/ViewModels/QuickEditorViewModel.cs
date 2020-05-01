using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Utils;
using ApiManager.ViewModels;
using Wpf.Util.Core.Command;

namespace ApiManager.QuickTest.ViewModels
{
	class QuickEditorViewModel
	{
		public QuickEditorViewModel(IEnumerable<ApiCommandDetail> commandDetails, Environment environment)
		{
			this.Commands = commandDetails.GroupBy(cmd => cmd.Command, cmd => cmd)
				.OrderBy(c => c.Key)
				.Select(c => new CommandInfoViewModel(c.Key, c.Select(v => v.SubCommand).ToArray()));

			//this.Commands = commandInfo.ApiCommands
			//	.OrderBy(c => c.Key)
			//	.Select(c => new CommandInfoViewModel(c.Key, c.Value));
			this.EditorViewModel = new HttpRequestEditorViewModel(environment);
		}

		public IEnumerable<CommandInfoViewModel> Commands { get; set; }
		public HttpRequestEditorViewModel EditorViewModel { get; private set; }
	}
}
