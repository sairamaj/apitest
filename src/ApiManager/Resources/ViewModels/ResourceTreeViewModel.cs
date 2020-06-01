using ApiManager.Resources.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceTreeViewModel : TreeViewItemViewModel
	{
		public ResourceTreeViewModel(ResourceData resource) : base(null, resource.Name, true)
		{
			this.Resource = resource;
		}

		public ResourceData Resource { get; }
	}
}
