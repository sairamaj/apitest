using System.Collections.Generic;
using System.Linq;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class DynamicVariableContainerInfoViewModel : CommandTreeViewModel
	{
		public DynamicVariableContainerInfoViewModel(DynamicVariableInfo dynamicVariableInfo) : base(null, "dynamic_variables", "dynamic_variables")
		{
			this.DynamicVariablesInfo = dynamicVariableInfo.DynamicVariables
				.OrderBy(f => f.Name)
				.Select(b => new DynamicVaribleInfoViewModel(b));
			this.IsExpanded = true;
		}

		public IEnumerable<DynamicVaribleInfoViewModel> DynamicVariablesInfo { get; }

		protected override void LoadChildren()
		{
			foreach (var info in DynamicVariablesInfo)
			{
				this.Children.Add(info);
			}
		}
	}
}
