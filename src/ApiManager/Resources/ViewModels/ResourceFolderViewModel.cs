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

		public ResourceFolderViewModel(ResourceTreeViewModel parent, string method, ResourceData resourceData)
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
				child = new ResourceFolderViewModel(this, _method, resource);
			}
			else
			{
				child = new ResourceViewModel(this, resource);
			}
			this.Children.Add(child);
			child.SelectionChanged += (s, e) =>
			{
				this.PropagateToParent(e);
			};
		}

		public ICommand NewFileCommand { get; set; }
		public ICommand NewFolderCommand { get; set; }
	}
}
