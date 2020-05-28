using System.Linq;
using ApiManager.Resources.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceFolderViewModel : TreeViewItemViewModel
	{
		public ResourceFolderViewModel(ResourceData resourceData): base(null, resourceData.Name, true)
		{
			ResourceData = resourceData;
		}

		public ResourceData ResourceData { get; }

		protected override void LoadChildren()
		{
			this.ResourceData.Children
				.Where(r => !r.IsContainer).ToList()
				.ForEach(r1 => this.Children.Add(new ResourceViewModel(r1)));

			this.ResourceData.Children
				.Where(r => r.IsContainer).ToList()
				.ForEach(r1 => this.Children.Add(new ResourceFolderViewModel(r1)));
		}
	}
}
