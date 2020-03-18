using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class CommandFileViewModel : CoreViewModel
	{
		private IEnumerable<string> _apis;
		public CommandFileViewModel(string fileName)
		{
			this.FileName = fileName;
			this.Name = Path.GetFileNameWithoutExtension(fileName);
			this.EditCommandFileCommand = new DelegateCommand(() =>
		   {
			   try
			   {
				   Process.Start(fileName);
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
		public IEnumerable<string> Apis
		{
			get
			{
				if (_apis == null)
				{
					if (File.Exists(this.FileName))
					{
						this._apis = File.ReadAllLines(this.FileName);
					}
				}

				return this._apis;
			}
		}
	}
}
