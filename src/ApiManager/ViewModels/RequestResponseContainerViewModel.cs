using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;
using ApiManager.Model;
using ApiManager.Utils;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;
using System.IO;

namespace ApiManager.ViewModels
{
	class RequestResponseContainerViewModel : CoreViewModel
	{
		public RequestResponseContainerViewModel(ObservableCollection<InfoViewModel> apiInfos)
		{
			this.ApiInfos = apiInfos;
			this.ClearCommand = new DelegateCommand(this.Clear);

			this.ApiInfos.CollectionChanged += (s, e) =>
			{
				OnPropertyChanged(() => this.ApisCount);
			};


			this.GenerateReportCommand = new DelegateCommand(() =>
			{
				UiHelper.SafeAction(() =>
			   {
				   var tempFolder = Path.Combine(FileHelper.GetTempPath(), "report");
				   var apis = this.ApiInfos.OfType<ApiInfoViewModel>().Select(a => a.ApiInfo);
				   Report.ReportGenerator.Generate(apis, tempFolder);
				   Process.Start(tempFolder);
			   }, "Unable to save");
			});
		}

		public ObservableCollection<InfoViewModel> ApiInfos { get; }
		public int ApisCount { get { return this.ApiInfos.Count; } }
		public ICommand ClearCommand { get; }
		public ICommand GenerateReportCommand { get; }
		public void Clear()
		{
			this.ApiInfos.Clear();
		}
	}
}
