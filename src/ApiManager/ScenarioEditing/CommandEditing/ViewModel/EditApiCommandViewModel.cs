using System.Linq;
using System.Windows;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ViewModels;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class EditApiCommandViewModel : DialogViewModel
	{
		private string _selectedMethod;
		public EditApiCommandViewModel(Window win, ApiScenarioItem apiItem) : base(win)
		{
			ApiItem = apiItem;
			this._selectedMethod = "Get";
		}

		public ApiScenarioItem ApiItem { get; }
		public string[] Methods => new string[] { "Get", "Post", "Put", "Patch", "Delete" };

		public string SelectedMethod
		{
			get
			{
				return this._selectedMethod;
			}
			set
			{
				this._selectedMethod = value;
				OnPropertyChanged(() => this.SelectedMethod);
				OnPropertyChanged(() => this.PayLoadFiles);
			}
		}

		public string SelectedPayLoad { get; set; }

		public string[] PayLoadFiles
		{
			get
			{
				return new string[] { "file1", "file2", "file3" }
				.Select(s => $"{this.SelectedMethod}-{s}")
				.ToArray();
			}
		}
	}
}
