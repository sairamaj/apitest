using ApiManager.Model;

namespace ApiManager.ViewModels
{
	class ApiExecuteInfoViewModel : InfoViewModel
	{
		public ApiExecuteInfoViewModel(ApiExecuteInfo apiExecuteInfo) : base(null, apiExecuteInfo)
		{
			this.ApiExecuteInfo = apiExecuteInfo;
		}

		public ApiExecuteInfo ApiExecuteInfo { get; }
	}
}
