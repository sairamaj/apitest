using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ScenarioContainerViewModel : CommandTreeViewModel
	{
		private IEnumerable<string> _apis;
		private readonly ApiInfo _apiInfo;
		private readonly IDataRepository _repository;

		public ScenarioContainerViewModel(Scenario scenario, ApiInfo apiInfo, IDataRepository repository)
			:base(null, scenario.Name, scenario.Name)
		{
			this.Scenario = scenario;
			this._apiInfo = apiInfo;
			this._repository = repository;
			this.FileName = scenario.FileName;
			this.Name = scenario.Name;
			this.IsExpanded = true;
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

		protected override void LoadChildren()
		{
			this.Load();		
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

		public Scenario Scenario { get; }

		private void Load()
		{
			this.Children.Clear();
			foreach (var child in this.Scenario.Children)
			{
				this.Children.Add(new ScenarioViewModel(child,  (newScenario)=>
				{
					this.Children.Add(new ScenarioViewModel(newScenario, (s) => { }, this._apiInfo, this._repository));
				}, this._apiInfo, this._repository));
			}
		}
	}
}
