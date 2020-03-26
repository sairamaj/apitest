using ApiManager.Model;
using ApiManager.Repository;

namespace ApiManager.ViewModels
{
	class AssertInfoViewModel : InfoViewModel
	{
		public AssertInfoViewModel(IApiExecutor executor, AssertInfo assertInfo) 
			: base(executor, assertInfo)
		{
			this.AssertInfo = assertInfo;
		}

		public AssertInfo AssertInfo { get; set; }

	}
}
