using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourcePopOutWindowViewModel : CoreViewModel
	{
		public ResourcePopOutWindowViewModel(ResourceContainerViewModel resourceContainerViewModel)
		{
			ResourceContainerViewModel = resourceContainerViewModel;
		}

		public ResourceContainerViewModel ResourceContainerViewModel { get; }
	}
}
