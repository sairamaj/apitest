using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class DialogViewModel : CoreViewModel
	{
		public DialogViewModel(Window win)
		{
			this.OkCommand = new DelegateCommand(() =>
			{
				win.DialogResult = true;
				win.Close();
			});
			this.CancelCommand = new DelegateCommand(() =>
			{
				win.DialogResult = false;
				win.Close();
			});

		}

		public ICommand OkCommand { get; }
		public ICommand CancelCommand { get; }

	}
}
