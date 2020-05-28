using System.Windows;
using ApiManager.ViewModels;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ApiConfigEditing.ViewModels
{
	class AddRouteWindowViewModel : DialogViewModel
	{
		public AddRouteWindowViewModel(Window win): base(win)
		{
		}

		public string Name { get; set; }

		public string Path { get; set; }
	}
}
