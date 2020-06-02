using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ApiManager.Common;
using ApiManager.Repository;
using ApiManager.Resources.Model;
using ApiManager.Resources.Views;
using ApiManager.Utils;
using ICSharpCode.AvalonEdit.Editing;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceContainerViewModel : CoreViewModel
	{
		private bool _ispopupWindow;
		private readonly IResourceManager _resourceManager;
		private readonly string _method;
		public event EventHandler<TreeItemSelectionChangedEventArg> SelectionChanged;

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
			this.PopOutCommand = new DelegateCommand(() =>
			{
				var win = new ResourcePopOutWindow();
				win.DataContext = new ResourcePopOutWindowViewModel($"{method} Resources", this);
				this.IsPopupWindow = true;
				win.Show();
				win.Closed += (s, e) => { this.IsPopupWindow = false; };
			});
		}

		public ObservableCollection<TreeViewItemViewModel> Resources { get; set; }
		public ICommand NewFileCommand { get; set; }
		public ICommand NewFolderCommand { get; set; }
		public ICommand RefreshResourcesCommand { get; set; }
		public ICommand PopOutCommand { get; set; }
		public ResourceTreeViewModel CurrentSelectedViewModel { get; set; }
		public bool IsPopupWindow
		{
			get => _ispopupWindow;
			set
			{
				this._ispopupWindow = value;
				OnPropertyChanged(() => this.IsPopupWindow);
			}
		}
		private void Refresh()
		{
			this.Resources.Clear();
			this._resourceManager.GetResources(this._method).ToList().ForEach(r => AddToChild(null, r));
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
				newItem = new ResourceFolderViewModel(parent, this._method, resource, (a,v) => DoAction(a,v));
			}
			else
			{
				newItem = new ResourceViewModel(parent, resource, (a, v) => DoAction(a, v));
			}

			if (parent == null)
			{
				this.Resources.Add(newItem);
			}
			else
			{
				parent.IsExpanded = true;
				if (parent is ResourceViewModel fileView)
				{
					// if selected is file , then get parent of file view
					parent = fileView.Parent as ResourceTreeViewModel;
				}

				if (parent == null)
				{
					this.Resources.Add(newItem);
				}
				else
				{
					parent.Children.Add(newItem);
				}
			}

			newItem.SelectionChanged += (s, e) =>
			{
				if (e.IsSelected)
				{
					if (e.TreeViewItemViewModel is ResourceTreeViewModel resourceViewModel)
					{
						CurrentSelectedViewModel = e.TreeViewItemViewModel as ResourceTreeViewModel;
						if (this.SelectionChanged != null)
						{
							this.SelectionChanged(this, e);
						}
					}
				}
			};
		}

		private void AddNewResource(Func<string, ResourceData> func)
		{
			UiHelper.SafeAction(() =>
			{
				var parentPath = this.CurrentSelectedViewModel == null ? this._resourceManager.GetResourcePath(this._method) : CurrentSelectedViewModel.Resource.ContainerPath;
				AddToChild(CurrentSelectedViewModel, func(parentPath));
			}, "Error");
		}

		private void DoAction(ResourceAction action, ResourceTreeViewModel viewModel)
		{
			if (action == ResourceAction.Delete)
			{
				if (ResourceEditingHelper.DeleteResource(viewModel.Resource))
				{
					this.Resources.Remove(viewModel);
				}
			}
			else if (action == ResourceAction.Copy)
			{
				var newResource = ResourceEditingHelper.CopyResource(this._method, viewModel.Resource);
				if(newResource != null )
				{
					AddToChild(null, newResource);
				}
			}
		}
	}
}
