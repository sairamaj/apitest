using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Repository;
using ApiManager.Resources.Model;
using ApiManager.ScenarioEditing.CommandEditing.Views;
using ApiManager.ScenarioEditing.Models;
using ApiManager.ViewModels;
using Wpf.Util.Core.Command;

namespace ApiManager.ScenarioEditing.CommandEditing.ViewModel
{
	class EditApiCommandViewModel : DialogViewModel
	{
		private readonly IResourceManager _resourceManager;
		private string _selectedMethod;
		public EditApiCommandViewModel(
			Window win, 
			ApiScenarioItem apiItem,
			IResourceManager resourceManager) : base(win)
		{
			ApiItem = apiItem;
			this._resourceManager = resourceManager;
			this._selectedMethod = "Get";
			this.NewResourceCommand = new DelegateCommand(() =>
		   {
			   new NewApiResourceWindow().ShowDialog();
		   });
		}


		public ApiScenarioItem ApiItem { get; }
		public string[] Methods => new string[] { "Get", "Post", "Put", "Patch", "Delete" };

		public string Command
		{
			get
			{
				var cmd = this.ApiItem.Command;
				if (this.SelectedMethod == "Get")
				{
					return cmd;
				}

				if (this.SelectedMethod == "Delete")
				{
					return cmd + " delete";
				}

				return cmd + $" {this.SelectedMethod.ToLowerInvariant()} {this.SelectedPayLoad.Name}";
			}
		}

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

		public ICommand NewResourceCommand { get; }
		public ResourceData SelectedPayLoad { get; set; }

		public ResourceData[] PayLoadFiles
		{
			get
			{
				return this._resourceManager
					.GetResources(this.SelectedMethod)
					.ToArray();
			}
		}

		private bool IsPayLoadRequired {
			get
			{
				return new string[] { "Post", "Put", "Patch" }.Contains(this.SelectedMethod);
			}
		}

		protected override bool OnClosing()
		{
			if (this.SelectedPayLoad == null && IsPayLoadRequired)
			{
				MessageBox.Show($"{this.SelectedMethod} require pay load");
				return false;
			}

			this.ApiItem.Command = this.Command;
			return base.OnClosing();
		}
	}
}
