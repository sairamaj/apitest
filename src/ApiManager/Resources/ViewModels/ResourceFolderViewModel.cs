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

		public ResourceFolderViewModel(string method, ResourceData resourceData) : base(resourceData)
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
			this.ResourceData.Children
				.Where(r => !r.IsContainer).ToList()
				.ForEach(r1 => this.Children.Add(new ResourceViewModel(r1)));

			this.ResourceData.Children
				.Where(r => r.IsContainer).ToList()
				.ForEach(r1 => this.Children.Add(new ResourceFolderViewModel(this._method, r1)));
		}

		private void AddChild(ResourceData resource)
		{
			if (resource == null)
				return;

			this.IsExpanded = true;
			if (resource.IsContainer)
			{
				this.Children.Add(new ResourceFolderViewModel(_method, resource));
			}
			else
			{
				this.Children.Add(new ResourceViewModel(resource));
			}
		}

		public ICommand NewFileCommand { get; set; }
		public ICommand NewFolderCommand { get; set; }
	}
}
