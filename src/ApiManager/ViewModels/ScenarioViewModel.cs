using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.ScenarioEditing.ViewModels;
using ApiManager.ScenarioEditing.Views;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ScenarioViewModel : CoreViewModel
	{
		private IEnumerable<string> _apis;
		public ScenarioViewModel(Scenario scenario, ApiInfo apiInfo, IDataRepository repository)
		{
			this.FileName = scenario.FileName;
			this.Name = scenario.Name;
			this.EditCommandFileCommand = new DelegateCommand(async () =>
		   {
		   try
		   {
				   //var helpCommands = await repository.GetHelpCommands().ConfigureAwait(true);
				   //var apiCommands = await repository.GetCommands(apiInfo).ConfigureAwait(true);

				   //var view = new ScenarioEditorView { DataContext = new ScenarioEditorViewModel(scenario, helpCommands, apiCommands) };
				   //view.ShowDialog();
				   Process.Start(scenario.FileName);
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
