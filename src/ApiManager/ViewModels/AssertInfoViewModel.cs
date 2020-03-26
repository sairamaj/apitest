using ApiManager.Model;

namespace ApiManager.ViewModels
{
	class AssertInfoViewModel : InfoViewModel
	{
		public AssertInfoViewModel(AssertInfo assertInfo) : base(assertInfo)
		{
			this.AssertInfo = assertInfo;
		}

		public AssertInfo AssertInfo { get; set; }

	}
}
