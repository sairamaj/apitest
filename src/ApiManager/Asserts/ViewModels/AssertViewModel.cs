using System;
using System.Diagnostics;
using System.Windows.Input;
using ApiManager.Asserts.Model;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Asserts.ViewModels
{
	class AssertViewModel : CoreViewModel
	{
		public AssertViewModel(AssertData assertdata)
		{
			this.AssertData = assertdata;
			Action showDetailsAction = () => Process.Start("notepad.exe", assertdata.FileName);
			this.EditCommandFileCommand = new DelegateCommand(showDetailsAction);
		}

		public AssertData AssertData { get; }
		public ICommand EditCommandFileCommand { get; }
	}
}
