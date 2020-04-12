using System;
using System.Diagnostics;
using System.Windows.Input;
using ApiManager.Resources.Model;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceViewModel : CoreViewModel
	{
		public ResourceViewModel(ResourceData resourceData)
		{
			ResourceData = resourceData;
			Action showDetailsAction = () => Process.Start("notepad.exe", resourceData.FileName);
			this.EditCommandFileCommand = new DelegateCommand(showDetailsAction);

		}

		public ResourceData ResourceData { get; }
		public ICommand EditCommandFileCommand { get; }
	}
}
