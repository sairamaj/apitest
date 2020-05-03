using System;
using System.Diagnostics;
using System.Windows.Input;
using ApiManager.Asserts.Model;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Asserts.ViewModels
{
	class VariableGroupViewModel : CoreViewModel
	{
		public VariableGroupViewModel(VariableGroupData groupData)
		{
			this.VariableGroupData = groupData;
			Action showDetailsAction = () => Process.Start("notepad.exe", groupData.FileName);
			this.EditCommandFileCommand = new DelegateCommand(showDetailsAction);
		}

		public VariableGroupData VariableGroupData { get; }
		public ICommand EditCommandFileCommand { get; }
	}
}
