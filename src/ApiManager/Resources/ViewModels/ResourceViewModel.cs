﻿using System;
using System.Windows;
using System.Windows.Input;
using ApiManager.Resources.Model;
using ApiManager.Resources.Views;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceViewModel : ResourceTreeViewModel
	{
		public ResourceViewModel(ResourceData resourceData) : base(resourceData)
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
