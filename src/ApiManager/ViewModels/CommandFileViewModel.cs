using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class CommandFileViewModel : CoreViewModel
	{
		public CommandFileViewModel(string fileName)
		{
			this.FileName = fileName;
			this.Name = Path.GetFileNameWithoutExtension(fileName);
			this.EditCommandFileCommand = new DelegateCommand(() =>
		   {
			   Process.Start(fileName);
		   });
		}

		public string Name { get; }
		public string FileName { get; }
		public ICommand EditCommandFileCommand { get; }
	}
}
