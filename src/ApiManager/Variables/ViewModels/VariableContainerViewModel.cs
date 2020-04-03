using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core;

namespace ApiManager.Variables.ViewModels
{
	class VariableContainerViewModel
	{
		private readonly IVariableManager _variableManager;

		public VariableContainerViewModel(IVariableManager variableManager)
		{
			this._variableManager = variableManager ?? throw new System.ArgumentNullException(nameof(variableManager));
			this.SystemVariables = variableManager.GetSystemVariables()
				.Select(kv => new VariableViewModel(kv.Key, kv.Value))
				.ToList();
			this.ApiVariables = new SafeObservableCollection<VariableViewModel>();
			this.EnvironmentVariables = new SafeObservableCollection<VariableViewModel>();
		}

		public IEnumerable<VariableViewModel> SystemVariables { get; }
		public ObservableCollection<VariableViewModel> ApiVariables { get; }
		public ObservableCollection<VariableViewModel> EnvironmentVariables { get; }

		public void UpdateApiVariables(ApiInfo apiInfo)
		{
			this.ApiVariables.Clear();
			foreach (var kv in this._variableManager.Get(apiInfo))
			{
				this.ApiVariables.Add(new VariableViewModel(kv.Key, kv.Value));
			}
		}

		public void UpdateEnvironmentVariables(Environment environment)
		{
			this.EnvironmentVariables.Clear();
			foreach (var kv in this._variableManager.Get(environment))
			{
				this.EnvironmentVariables.Add(new VariableViewModel(kv.Key, kv.Value));
			}
		}
	}
}
