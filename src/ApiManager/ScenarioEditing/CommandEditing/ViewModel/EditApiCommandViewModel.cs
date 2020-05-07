using System.Windows;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ViewModels;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class EditApiCommandViewModel : DialogViewModel
	{
		public EditApiCommandViewModel(Window win, ApiScenarioItem apiItem) : base(win)
		{
			ApiItem = apiItem;
		}

		public ApiScenarioItem ApiItem { get; }
	}
}
