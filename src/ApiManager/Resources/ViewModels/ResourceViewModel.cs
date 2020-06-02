using System;
using System.Windows.Input;
using ApiManager.Resources.Model;
using ApiManager.Resources.Views;
using ApiManager.Utils;
using Wpf.Util.Core.Command;

namespace ApiManager.Resources.ViewModels
{
	class ResourceViewModel : ResourceTreeViewModel
	{
		public ResourceViewModel(ResourceTreeViewModel parent, ResourceData resourceData) : base(parent, resourceData)
		{
			ResourceData = resourceData;
			Action showDetailsAction = () =>
			{
				UiHelper.SafeAction(() =>
			   { 
				   var win = new ViewResourceWindow
				   {
					   DataContext = new ViewResourceViewModel(resourceData.FileName)
				   };
				   win.ShowDialog();
			   }, "View");
			};

			this.EditCommandFileCommand = new DelegateCommand(showDetailsAction);
			this.IsExpanded = true;
		}

		public ResourceData ResourceData { get; }
		public ICommand EditCommandFileCommand { get; }
	}
}
