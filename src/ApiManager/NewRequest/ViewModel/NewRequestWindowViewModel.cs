using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Extensions;
using ApiManager.Model;
using ApiManager.NewRequest.Views;
using ApiManager.Repository;
using ApiManager.ViewModels;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.NewRequest.ViewModel
{
	class NewRequestWindowViewModel : CoreViewModel
	{
		private ApiViewModel _selectedApiInfoViewModel;
		private EnvironmentViewModel _selectedEnvironment;
		private readonly ICommandExecutor _executor;
		private IDataRepository _dataRepository;
		private string _selectedApi;
		public NewRequestWindowViewModel(ICommandExecutor executor, IDataRepository dataRepository)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
			_dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));

			this.SelectedMethod = HttpMethods.First();
			//this.Url = "https://api.enterprise.apigee.com/v1/organizations/sairamaj-eval/apis";
			this.SubmitCommand = new DelegateCommand(async () => await Submit().ConfigureAwait(false));

			this.ApiInfoViewModels = new SafeObservableCollection<ApiViewModel>();
			foreach (var envInfo in this._dataRepository.GetApiConfigurations())
			{
				this.ApiInfoViewModels.Add(new ApiViewModel(envInfo, this._dataRepository, this._executor));
			}

			this.SelectedApiInfoViewModel = this.ApiInfoViewModels.FirstOrDefault();
			this.AuthenticateCommand = new DelegateCommand(async () => await this.Authenticate().ConfigureAwait(false));
			//
			this.HeaderItems = new HeaderItemsViewModel(new Dictionary<string, string> {
			});
		}

		public string[] HttpMethods => new string[] { "GET", "POST", "PUT", "PATCH", "DELETE" };
		public string SelectedMethod { get; set; }
		public string SelectedApi
		{
			get => this._selectedApi;
			set
			{
				this._selectedApi = value;
				this.UpdateUrl();
			}
		}
		public IEnumerable<ApiRoute> Routes { get; set; }
		public string Url { get; set; }
		public ICommand SubmitCommand { get; }
		public string Response { get; set; }
		public ApiRequest ApiRequest { get; set; }
		public ObservableCollection<ApiViewModel> ApiInfoViewModels { get; set; }
		public IEnumerable<EnvironmentViewModel> Environments { get; set; }
		public string[] Apis { get; private set; }
		public HeaderItemsViewModel HeaderItems { get; }
		public bool IsSuccess { get; set; }

		public EnvironmentViewModel SelectedEnvironment
		{
			get
			{
				return this._selectedEnvironment;
			}
			set
			{
				this._selectedEnvironment = value;
				UpdateUrl();
			}
		}

		public ICommand AuthenticateCommand { get; }
		public ApiViewModel SelectedApiInfoViewModel
		{
			get
			{
				return this._selectedApiInfoViewModel;
			}
			set
			{
				this._selectedApiInfoViewModel = value;
				if (value == null)
				{
					return;
				}

				this.OnApiConfigSelectionChange();
			}
		}

		private async void OnApiConfigSelectionChange()
		{
			this.Environments = this.SelectedApiInfoViewModel.ApiInfo.Environments
				.Select(v => new EnvironmentViewModel(this.SelectedApiInfoViewModel.ApiInfo, v, this._dataRepository));
			OnPropertyChanged(() => this.Environments);
			var commands = await this._dataRepository.GetCommands(this.SelectedApiInfoViewModel.ApiInfo).ConfigureAwait(true);
			this.Routes = commands.ApiCommands.SelectMany(c => c.Routes.Select(r=> r));
			this.Apis = commands.ApiCommands.SelectMany(c => c.Routes.Select(r => r.FullName)).ToArray();
			this.SelectedEnvironment = this.Environments.FirstOrDefault();
			OnPropertyChanged(() => this.SelectedEnvironment);
			OnPropertyChanged(() => this.Apis);
		}

		private async Task Submit()
		{
			try
			{

				var apiRequest = new ApiRequest();
				apiRequest.Method = this.SelectedMethod;
				apiRequest.Request = new Request();
				apiRequest.Request.Headers = this.HeaderItems.Items.ToDictionary(vm => vm.Name, vm => vm.Value);
				apiRequest.Url = this.Url;

				var request = new HttpRequestClient(apiRequest);
				this.ApiRequest = await request.GetResponseAsync().ConfigureAwait(false);
				this.Response = this.ApiRequest?.Response?.Content;
				this.IsSuccess = this.ApiRequest?.HttpCode <= 299 ? true : false;

				OnPropertyChanged(() => this.Response);
				OnPropertyChanged(() => this.ApiRequest);
				OnPropertyChanged(() => this.IsSuccess);
			}
			catch (Exception e)
			{
				this.Response = e.ToString();
				OnPropertyChanged(() => this.Response);
			}
		}

		private void UpdateUrl()
		{
			var route = this.Routes.FirstOrDefault(r => r.FullName == this.SelectedApi);
			if (route == null)
			{
				return;
			}

			var variables = this.SelectedEnvironment.Variables;
			var url = route.FullUrl;
			foreach (var variable in this.SelectedEnvironment.Variables)
			{
				var replaceVariable = "{{" + $"{variable.Key}" + "}}";
				url = url.Replace(replaceVariable, variable.Value);
			}
			this.Url = url;
			OnPropertyChanged(() => this.Url);
		}

		private async Task Authenticate()
		{
			var commands = await this._dataRepository.GetCommands(this.SelectedApiInfoViewModel.ApiInfo).ConfigureAwait(true);
			var viewModel = new AuthenticationViewModel(commands.ApiCommands.First(), this.SelectedEnvironment.Environment);

			new AuthenticateWindow()
			{
				DataContext = viewModel
			}.ShowDialog();

			this.HeaderItems.Add("Authorization", $"Bearer {viewModel.AccessToken}");
		}
	}
}
 