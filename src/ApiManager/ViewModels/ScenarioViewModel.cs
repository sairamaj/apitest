using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ScenarioViewModel : CommandTreeViewModel
	{
		private IEnumerable<string> _apis;
		private IDataRepository _repository;
		private ApiInfo _apiInfo;
		private Action<Scenario> _onEvent;
		public ScenarioViewModel(
			Scenario scenario, 
			Action<Scenario> onEvent,
			ApiInfo apiInfo, 
			IDataRepository repository)
			: base(null, scenario.Name, scenario.Name)
		{
			this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this._onEvent = onEvent ?? throw new ArgumentNullException(nameof(onEvent));
			this._apiInfo = apiInfo;

			this.Scenario = scenario;
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

			this.CopyCommand = new DelegateCommand(() => this.CopyScenario());
			this.IsExpanded = true;
		}

		public string Name { get; }
		public string FileName { get; }
		public ICommand EditCommandFileCommand { get; }
		public ICommand CopyCommand { get; }
		public Scenario Scenario { get; }
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

		protected override void LoadChildren()
		{
			foreach (var container in this.Scenario.Children.Where(s => s.IsContainer))
			{
				this.Children.Add(new ScenarioContainerViewModel(container, this._apiInfo, this._repository));
			}
		}

		private void CopyScenario()
		{
			var newScenario = this._repository.CopyScenario(this.Scenario);
			this._onEvent(newScenario);
		}
	}
}
