using System;
using System.Diagnostics;
using System.Windows.Input;
using ApiManager.Scripts.Models;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Scripts.ViewModels
{
	class ScriptViewModel : CoreViewModel
	{
		public ScriptViewModel(ScriptData scriptData)
		{
			this.ScriptData = scriptData;
			Action showDetailsAction = () => JavaScriptUiHelper.ShowJavaScriptWindow(scriptData.FileName);
			this.EditCommandFileCommand = new DelegateCommand(showDetailsAction);
		}

		public ScriptData ScriptData { get; }
		public ICommand EditCommandFileCommand { get; }
	}
}
