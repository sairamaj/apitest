using ApiManager.ScenarioEditing.Models;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.ViewModel
{
	class ScenarioLineItemViewModel : CoreViewModel
	{
		public ScenarioLineItemViewModel(ScenarioLineItem lineItem)
		{
			this. LineItem = lineItem ?? throw new System.ArgumentNullException(nameof(lineItem));
		}

		public ScenarioLineItem LineItem { get; }
	}
}
