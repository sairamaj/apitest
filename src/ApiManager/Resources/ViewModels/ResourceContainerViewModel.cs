using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceContainerViewModel
	{
		private readonly IResourceManager _resourceManager;
		private readonly string _method;

		public ResourceContainerViewModel(IResourceManager resourceManager, string method)
		{
			this._resourceManager = resourceManager;
			this._method = method;

			this.Resources = new SafeObservableCollection<TreeViewItemViewModel>();
			this.Refresh();
			this.NewFileCommand = new DelegateCommand(() =>
			{
				var newResource = ResourceEditingHelper.CreateNewResource(this._method, resourceManager.GetResourcePath(method));
				if (newResource != null)
				{
					this.Resources.Add(new ResourceViewModel(newResource));
				}
			});
			this.NewFolderCommand = new DelegateCommand(() =>
			{
				var newResource = ResourceEditingHelper.CreateNewResourceFolder(this._method, resourceManager.GetResourcePath(method));
				if (newResource != null)
				{
					this.Resources.Add(new ResourceFolderViewModel(newResource));
				}
			});

			this.RefreshResourcesCommand = new DelegateCommand(this.Refresh);
		}

		public ObservableCollection<TreeViewItemViewModel> Resources { get; set; }
		public ICommand NewFileCommand { get; set; }
		public ICommand NewFolderCommand { get; set; }
		public ICommand RefreshResourcesCommand { get; set; }

		private void Refresh()
		{
			this.Resources.Clear();
			var resources = this._resourceManager.GetResources(this._method).ToList();
			resources
				.Where(r => !r.IsContainer).Select(r => new ResourceViewModel(r))
				.ToList().ForEach(r1 => this.Resources.Add(r1));

			resources
				.Where(r => r.IsContainer).Select(r => new ResourceFolderViewModel(r))
				.ToList().ForEach(r1 => this.Resources.Add(r1));
		}
	}
}
