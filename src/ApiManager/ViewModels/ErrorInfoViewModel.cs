using ApiManager.Model;

namespace ApiManager.ViewModels
{
	class ErrorInfoViewModel : InfoViewModel
	{
		public ErrorInfoViewModel(ErrorInfo errorInfo) : base(null, errorInfo)
		{
			this.ErrorInfo = errorInfo;
		}

		public ErrorInfo ErrorInfo { get; }
	}
}
