using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Resources.Model;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceFolderViewModel : ResourceTreeViewModel
	{
		private readonly string _method;

		public ResourceFolderViewModel(
			ResourceTreeViewModel parent, 
			string method, 
			ResourceData resourceData,
			Action<ResourceAction, ResourceTreeViewModel> onAction)
			: base(parent, resourceData)
		{
			this._method = method;
			ResourceData = resourceData;
			this.NewFileCommand = new DelegateCommand(() =>
		   {
			   UiHelper.SafeAction(() =>
			  {
				  AddChild(ResourceEditingHelper.CreateNewResource(method, resourceData.ContainerPath));
			  }, "Error");
		   });
			this.NewFolderCommand = new DelegateCommand(() =>
			{
				AddChild(ResourceEditingHelper.CreateNewResourceFolder(method, resourceData.ContainerPath));
			});
			this.DeleteFolderCommand = new DelegateCommand(() =>
			{
				onAction(ResourceAction.Delete, this);
		   });

		}

		public ResourceData ResourceData { get; }

		protected override void LoadChildren()
		{

			this.ResourceData.Children.ToList().ForEach(r => AddChild(r));
		}

		private void AddChild(ResourceData resource)
		{
			if (resource == null)
				return;

			this.IsExpanded = true;
			ResourceTreeViewModel child;
			if (resource.IsContainer)
			{
				child = new ResourceFolderViewModel(this, _method, resource, (a, v) => DoAction(a, v));
			}
			else
			{
				child = new ResourceViewModel(this, resource, (a, v) => DoAction(a, v));
			}

			this.Children.Add(child);
			child.SelectionChanged += (s, e) =>
			{
				this.PropagateToParent(e);
			};
		}

		public ICommand NewFileCommand { get; set; }
		public ICommand NewFolderCommand { get; set; }
		public ICommand DeleteFolderCommand { get; set; }

		private void DoAction(ResourceAction action, ResourceTreeViewModel viewModel)
		{
			if (action == ResourceAction.Delete)
			{
				if (ResourceEditingHelper.DeleteResource(viewModel.Resource))
				{
					this.Children.Remove(viewModel);
				}
			}
			else if (action == ResourceAction.Copy)
			{
				var newResource = ResourceEditingHelper.CopyResource(this._method, viewModel.Resource);
				if (newResource != null)
				{
					AddChild(newResource);
				}
			}
		}
	}
}
