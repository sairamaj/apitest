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
		private readonly ICacheManager _cacheManager;
		private string _selectedApi;
		public NewRequestWindowViewModel(
			ICommandExecutor executor,
			IDataRepository dataRepository,
			ICacheManager cacheManager
			)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
			_dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
			this._cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
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

			AddHeaders();
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
		public HeaderItemsViewModel HeaderItems { get; private set; }
		public HeaderItemsViewModel ResponseHeaderItems { get; private set; }
		public string RequestContent { get; set; }


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
			this.Routes = commands.ApiCommands.SelectMany(c => c.Routes.Select(r => r));
			this.Apis = commands.ApiCommands.SelectMany(c => 
			c.Routes.Where(r => r.ApiName != "accesstoken")
			.Select(r => r.FullName)).ToArray();
			this.SelectedEnvironment = this.Environments.FirstOrDefault();
			this.SelectedApi = this.Apis.FirstOrDefault();

			OnPropertyChanged(() => this.SelectedEnvironment);
			OnPropertyChanged(() => this.Apis);
			OnPropertyChanged(() => this.SelectedApi);
		}

		private void AddHeaders()
		{
			var initialHeaders = new Dictionary<string, string>
			{
			};
			var cachedAccessToken = this._cacheManager.Get("AccessToken");
			if (cachedAccessToken != null)
			{
				initialHeaders["Authorization"] = $"Bearer {cachedAccessToken}";
			}

			initialHeaders["Content-Type"] = "application/json";

			this.HeaderItems = new HeaderItemsViewModel(initialHeaders);
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
				apiRequest.Request.Body = RequestContent;

				var request = new HttpRequestClient(apiRequest);
				this.ApiRequest = await request.GetResponseAsync().ConfigureAwait(false);
				if (this.ApiRequest != null)
				{
					this.Response = this.ApiRequest?.Response?.Content;
					this.IsSuccess = this.ApiRequest?.HttpCode <= 299 ? true : false;
					this.ResponseHeaderItems = new HeaderItemsViewModel(this.ApiRequest.Response.Headers);
				}
				
				OnPropertyChanged(() => this.Response);
				OnPropertyChanged(() => this.ApiRequest);
				OnPropertyChanged(() => this.IsSuccess);
				OnPropertyChanged(() => this.ResponseHeaderItems);
				
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
			this.Url = Evaluator.Evaluate(route.FullUrl, variables);
			OnPropertyChanged(() => this.Url);
		}

		private async Task Authenticate()
		{
			var commands = await this._dataRepository.GetCommands(this.SelectedApiInfoViewModel.ApiInfo).ConfigureAwait(true);
			var viewModel = new AuthenticationViewModel(
				this._cacheManager,
				commands.ApiCommands.First(),
				this.SelectedEnvironment.Environment
				);

			new AuthenticateWindow()
			{
				DataContext = viewModel
			}.ShowDialog();

			if (!string.IsNullOrWhiteSpace(viewModel.AccessToken))
			{
				this.HeaderItems.Add("Authorization", $"Bearer {viewModel.AccessToken}");
			}
		}
	}
}
