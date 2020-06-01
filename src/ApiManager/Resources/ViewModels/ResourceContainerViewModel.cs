using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Repository;
using ApiManager.Resources.Model;
using ApiManager.Resources.Views;
using ApiManager.Utils;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.Extensions;
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
				AddNewResource(parentPath => ResourceEditingHelper.CreateNewResource(method, parentPath));
			});
			this.NewFolderCommand = new DelegateCommand(() =>
			{
				AddNewResource(parentPath => ResourceEditingHelper.CreateNewResourceFolder(method, parentPath));
			});

			this.RefreshResourcesCommand = new DelegateCommand(this.Refresh);
			this.PopOutCommand = new DelegateCommand(() => {
				var win = new ResourcePopOutWindow();
				win.DataContext = this;
				win.Show();
			});
		}

		public ObservableCollection<TreeViewItemViewModel> Resources { get; set; }
		public ICommand NewFileCommand { get; set; }
		public ICommand NewFolderCommand { get; set; }
		public ICommand RefreshResourcesCommand { get; set; }
		public ICommand PopOutCommand { get; set; }

		private void Refresh()
		{
			this.Resources.Clear();
			var resources = this._resourceManager.GetResources(this._method).ToList();
			resources
				.Where(r => !r.IsContainer).Select(r => new ResourceViewModel(r))
				.ToList().ForEach(r1 => this.Resources.Add(r1));

			resources
				.Where(r => r.IsContainer).Select(r => new ResourceFolderViewModel(this._method, r))
				.ToList().ForEach(r1 => this.Resources.Add(r1));
		}

		private void AddToChild(ResourceTreeViewModel parent, ResourceData resource)
		{
			if (resource == null)
			{
				return;
			}

			ResourceTreeViewModel newItem;
			if (resource.IsContainer)
			{
				newItem = new ResourceFolderViewModel(this._method, resource);
			}
			else
			{
				newItem = new ResourceViewModel(resource);
			}

			if (parent == null)
			{
				this.Resources.Add(newItem);
			}
			else
			{
				parent.IsExpanded = true;
				parent.Children.Add(newItem);
			}
		}

		private ResourceTreeViewModel GetSelectedResource()
		{
			var allResources = this.Resources.Flatten(c => c.Children.OfType<ResourceTreeViewModel>());
			foreach (var resourceViewModel in allResources.OfType<ResourceTreeViewModel>())
			{
				if (resourceViewModel.IsSelected)
				{
					return resourceViewModel;
				}
			}

			return null;
		}

		private void AddNewResource(Func<string, ResourceData> func)
		{
			UiHelper.SafeAction(() =>
			{
				var selectedResource = GetSelectedResource();
				var parentPath = selectedResource == null ? this._resourceManager.GetResourcePath(this._method) : selectedResource.Resource.ContainerPath;
				AddToChild(selectedResource, func(parentPath));
			}, "Error");
		}
	}
}
