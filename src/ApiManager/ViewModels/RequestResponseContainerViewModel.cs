using System.Collections.ObjectModel;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class RequestResponseContainerViewModel : CoreViewModel
	{
		public RequestResponseContainerViewModel(ObservableCollection<ApiInfo> apiInfos)
		{
			this.ApiInfos = apiInfos;
		}

		public ObservableCollection<ApiInfo> ApiInfos { get; }
	}
}
