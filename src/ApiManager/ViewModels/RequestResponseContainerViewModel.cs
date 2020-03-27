using System.Collections.ObjectModel;
using System.Windows.Input;
using ApiManager.Model;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

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
		}

		public ObservableCollection<InfoViewModel> ApiInfos { get; }
		public int ApisCount { get { return this.ApiInfos.Count; } }
		public ICommand ClearCommand { get; }
		public void Clear()
		{
			this.ApiInfos.Clear();
		}
	}
}
