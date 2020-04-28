using ApiManager.Model;
using ApiManager.Repository;

namespace ApiManager.ViewModels
{
	class PrintInfoViewModel : InfoViewModel
	{
		public PrintInfoViewModel(PrintInfo printInfo) 
			: base(null, printInfo)
		{
			this.PrintInfo = printInfo;
		}

		public PrintInfo PrintInfo { get; set; }

	}
}
