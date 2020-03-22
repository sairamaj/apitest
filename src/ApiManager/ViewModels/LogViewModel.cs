using System.Collections.ObjectModel;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class LogViewModel : CoreViewModel
	{
		public LogViewModel()
		{
			this.LogMessages = new SafeObservableCollection<string>();
		}
		public ObservableCollection<string> LogMessages { get; set; }

		public void Add(string message)
		{
			this.LogMessages.Add(message);
		}
	}
}
