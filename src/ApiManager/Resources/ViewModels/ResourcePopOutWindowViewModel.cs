using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourcePopOutWindowViewModel : CoreViewModel
	{
		public ResourcePopOutWindowViewModel(ResourceContainerViewModel resourceContainerViewModel)
		{
			this.ResourceContainerViewModel = resourceContainerViewModel;
			this.ResourceContainerViewModel.SelectionChanged += (s, e) => this.OnResourceChanged(e.TreeViewItemViewModel as ResourceTreeViewModel);
		}

		public ResourceContainerViewModel ResourceContainerViewModel { get; }
		public ViewResourceViewModel CurrentResourceDataViewModel { get; set; }
		private void OnResourceChanged(ResourceTreeViewModel viewModel)
		{
			this.CurrentResourceDataViewModel = new ViewResourceViewModel(viewModel.Resource.FileName);
			OnPropertyChanged(() => this.CurrentResourceDataViewModel);
		}
	}
}
