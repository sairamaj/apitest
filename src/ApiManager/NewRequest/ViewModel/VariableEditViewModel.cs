using System;
using System.Collections.Generic;
using System.Linq;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.NewRequest.ViewModel
{
	class VariableEditViewModel : CoreViewModel
	{
		private readonly IDictionary<string, string> variables;

		public VariableEditViewModel(IDictionary<string,string> variables, Action<string,string> onPropertyChanged)
		{
			this.Variables = variables.Select(kv => new VariableNameValueViewModel(kv.Key, kv.Value)).ToList();
			foreach (var variable in this.Variables)
			{
				variable.PropertyChanged += (s, e) =>
				{
					var viewModel = s as VariableNameValueViewModel;
					onPropertyChanged(viewModel.Name, viewModel.Value);
				};
			}

		}

		public IEnumerable<VariableNameValueViewModel> Variables { get; set; }
		public IDictionary<string, string> VariableWithValues => this.Variables.ToDictionary(v => v.Name, v => v.Value);
	}
}
