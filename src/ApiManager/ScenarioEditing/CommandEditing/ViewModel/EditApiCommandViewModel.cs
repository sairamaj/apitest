using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Repository;
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
		private string _selectedPayload;
		public EditApiCommandViewModel(
			Window win,
			ApiScenarioItem apiItem,
			IResourceManager resourceManager) : base(win)
		{
			ApiItem = apiItem;
			this._resourceManager = resourceManager;
			this._selectedMethod = "Get";
			this.NewResourceCommand = new DelegateCommand(()=> this.EditResource(null));
			this.EditResourceCommand = new DelegateCommand(() =>
			{
				this.EditResource(Path.Combine(resourceManager.GetResourcePath(this.SelectedMethod), this.SelectedPayLoad));
			});
		}


		public ApiScenarioItem ApiItem { get; }
		public string[] Methods => new string[] { "Get", "Post", "Put", "Patch", "Delete" };

		public string Command
		{
			get
			{
				var cmd = this.ApiItem.ApiName;
				if (this.SelectedMethod == "Get")
				{
					return cmd;
				}

				if (this.SelectedMethod == "Delete")
				{
					return cmd + " delete";
				}

				return cmd + $" {this.SelectedMethod.ToLowerInvariant()} {this.SelectedPayLoad}";
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
		public ICommand EditResourceCommand { get; }

		public string SelectedPayLoad
		{
			get
			{
				return this._selectedPayload;
			}
			set
			{
				this._selectedPayload = value;
				OnPropertyChanged(() => this.SelectedPayLoad);
			}
		}

		public IEnumerable<string> PayLoadFiles
		{
			get
			{
				return this._resourceManager
					.GetResources(this.SelectedMethod)
					.Select(r => r.FileWithExtension);
			}
		}

		private bool IsPayLoadRequired
		{
			get
			{
				return new string[] { "Post", "Put", "Patch" }.Contains(this.SelectedMethod);
			}
		}

		protected override bool OnClosing()
		{
			if (string.IsNullOrWhiteSpace(this.SelectedPayLoad) && IsPayLoadRequired)
			{
				MessageBox.Show($"{this.SelectedMethod} require pay load");
				return false;
			}

			this.ApiItem.Command = this.Command;
			return base.OnClosing();
		}

		private void EditResource(string fileName)
		{
			var resourceWindow = new NewApiResourceWindow();
			var viewModel = new NewApiResourceViewModel(resourceWindow, this.SelectedMethod, fileName, ServiceLocator.Locator.Resolve<IResourceManager>());
			resourceWindow.DataContext = viewModel;
			if (resourceWindow.ShowDialog().Value)
			{
				OnPropertyChanged(() => this.PayLoadFiles);
				this.SelectedPayLoad = viewModel.Name;
				OnPropertyChanged(() => this.SelectedPayLoad);
			}
		}
	}
}
