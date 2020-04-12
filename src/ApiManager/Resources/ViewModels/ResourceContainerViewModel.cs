using System.Collections.Generic;
using System.Linq;
using ApiManager.Repository;

namespace ApiManager.Resources.ViewModels
{
	class ResourceContainerViewModel
	{
		public ResourceContainerViewModel(IResourceManager resourceManager, string method)
		{
			this.Resources = resourceManager.GetResources(method).Select(a => new ResourceViewModel(a));
		}

		public IEnumerable<ResourceViewModel> Resources { get; set; }
	}
}
