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
		public ResourceContainerViewModel(IResourceManager resourceManager, string method)
		{
			var resources = resourceManager.GetResources(method).ToList();
			this.Resources = new SafeObservableCollection<TreeViewItemViewModel>();
			resources
				.Where(r => !r.IsContainer).Select(r => new ResourceViewModel(r))
				.ToList().ForEach(r1 => this.Resources.Add(r1));

			resources
				.Where(r => r.IsContainer).Select(r => new ResourceFolderViewModel(r))
				.ToList().ForEach(r1 => this.Resources.Add(r1));


			this.NewFileCommand = new DelegateCommand(() => {
				MessageBox.Show($" New item 222: {method}");
				var newResource = ResourceEditingHelper.CreateNewResource(resourceManager.GetResourcePath(method));
				this.Resources.Add(new ResourceViewModel(newResource));
			});
			this.NewFolderCommand = new DelegateCommand(() => {
				MessageBox.Show($" New folder 333 {method}");
			});

		}

		public ObservableCollection<TreeViewItemViewModel> Resources { get; set; }
		public ICommand NewFileCommand { get; set; }
		public ICommand NewFolderCommand { get; set; }

	}
}
