using System.Collections.Generic;
using System.Linq;
using ApiManager.Repository;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceContainerViewModel
	{
		public ResourceContainerViewModel(IResourceManager resourceManager, string method)
		{
			var resources = resourceManager.GetResources(method).ToList();
			this.Resources = resources.Where(r => !r.IsContainer).Select(r => new ResourceViewModel(r));
			this.Resources = this.Resources.Union(resources.Where(r => r.IsContainer).Select(r => new ResourceFolderViewModel(r)));
		}

		public IEnumerable<TreeViewItemViewModel> Resources { get; set; }
	}
}
