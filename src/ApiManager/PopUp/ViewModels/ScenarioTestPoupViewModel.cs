using System.Collections.Generic;
using System.Linq;
using ApiManager.ViewModels;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.PopUp.ViewModels
{
	class ScenarioTestPoupViewModel : CoreViewModel
	{
		private ApiInfoViewModel _selectedApiInfo;
		public ScenarioTestPoupViewModel(IEnumerable<ApiInfoViewModel> apiInfoViews)
		{
			this.ApiInfoViews = apiInfoViews;
			this._selectedApiInfo = this.ApiInfoViews.FirstOrDefault();
		}

		public IEnumerable<ApiInfoViewModel> ApiInfoViews { get; }
		public ApiInfoViewModel SelectedApiInfo
		{
			get => this._selectedApiInfo; 
			set
			{
				this._selectedApiInfo = value;
				OnPropertyChanged(() => this.SelectedApiInfo);
			}
		}
	}
}
