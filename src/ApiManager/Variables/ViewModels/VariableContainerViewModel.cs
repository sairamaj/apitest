using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Variables.ViewModels
{
	class VariableContainerViewModel : CoreViewModel
	{
		private readonly IResourceManager _resourceManager;

		public VariableContainerViewModel(IResourceManager resourceManager)
		{
			this._resourceManager = resourceManager ?? throw new System.ArgumentNullException(nameof(resourceManager));
			this.SystemVariables = resourceManager.GetSystemVariables()
				.Select(kv => new VariableViewModel(kv.Key, kv.Value))
				.ToList();
			this.ApiVariables = new SafeObservableCollection<VariableViewModel>();
			this.EnvironmentVariables = new SafeObservableCollection<VariableViewModel>();
		}

		public IEnumerable<VariableViewModel> SystemVariables { get; }
		public ObservableCollection<VariableViewModel> ApiVariables { get; }
		public ObservableCollection<VariableViewModel> EnvironmentVariables { get; }
		public string CurrentEnvironmentName { get; set; }
		public string CurrentApiName { get; set; }
		public string EnvironmentNameTitle
		{
			get
			{
				return $"Environment({this.CurrentEnvironmentName})";
			}
		}
		public string ApiNameTitle
		{
			get
			{
				return $"Api({this.CurrentApiName})";
			}
		}

		public void UpdateApiVariables(ApiInfo apiInfo)
		{
			this.ApiVariables.Clear();
			foreach (var kv in this._resourceManager.Get(apiInfo))
			{
				this.ApiVariables.Add(new VariableViewModel(kv.Key, kv.Value));
			}

			this.CurrentApiName = apiInfo.Name;
			OnPropertyChanged(() => this.ApiNameTitle);
		}

		public void UpdateEnvironmentVariables(Environment environment)
		{
			this.EnvironmentVariables.Clear();
			foreach (var kv in this._resourceManager.Get(environment))
			{
				this.EnvironmentVariables.Add(new VariableViewModel(kv.Key, kv.Value));
			}

			this.CurrentEnvironmentName = environment.Name;
			OnPropertyChanged(() => this.EnvironmentNameTitle);
		}
	}
}
