using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.NewRequest.ViewModel
{
	class HeaderItemsViewModel : CoreViewModel
	{
		public HeaderItemsViewModel(IDictionary<string,string> items)
		{
			this.Items = new SafeObservableCollection<NameValueViewModel>();
			items.ToList().ForEach(kv => this.Items.Add(new NameValueViewModel(kv.Key, kv.Value)));
			this.GetCommand = new DelegateCommand(() => {
				foreach (var item in this.Items)
				{
					MessageBox.Show($"{item.Name}-{item.Value}");
				}
			});
		}

		public ObservableCollection<NameValueViewModel> Items { get; set; }
		public ICommand GetCommand { get; }

		public void Add(string name, string value)
		{
			this.Items.Add(new NameValueViewModel(name, value));
		}
	}
}
