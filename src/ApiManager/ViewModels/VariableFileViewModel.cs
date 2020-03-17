using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class VariableFileViewModel : CoreViewModel
	{
		public VariableFileViewModel(string fileName)
		{
			this.FileName = fileName;
			this.Name = Path.GetFileNameWithoutExtension(fileName);
			this.EditCommandFileCommand = new DelegateCommand(() =>
		   {
			   try
			   {
				   Process.Start("notepad", fileName);
			   }
			   catch (Exception e)
			   {
				   MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			   }
		   });
		}

		public string Name { get; }
		public string FileName { get; }
		public ICommand EditCommandFileCommand { get; }
	}
}
