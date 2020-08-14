using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Extensions;
using ApiManager.Model;
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
		}

		public string[] HttpMethods => new string[] { "GET", "POST", "PUT", "PATCH", "DELETE" };
		public string SelectedMethod { get; set; }
		public string Url { get; set; }
		public ICommand SubmitCommand { get; }
		public string Response { get; set; }
		public ApiRequest ApiRequest { get; set; }
		public ObservableCollection<ApiViewModel> ApiInfoViewModels { get; set; }
		public IEnumerable<EnvironmentViewModel> Environments { get; set; }
		public string[] Apis { get; private set; }

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
			this.Apis = commands.ApiCommands.SelectMany(c => c.Routes.Select(r =>
			{
				var route = r.Name == "_" ? string.Empty : $".{r.Name}";
				return $"{c.Name}{route}";
			})).ToArray();
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
				apiRequest.Request.Headers = new Dictionary<string, string>();
				apiRequest.Url = this.Url;

				var request = new HttpRequestClient(apiRequest);
				this.ApiRequest = await request.GetResponseAsync().ConfigureAwait(false);
				this.Response = this.ApiRequest.Response.Content;
				OnPropertyChanged(() => this.Response);
				OnPropertyChanged(() => this.ApiRequest);
			}
			catch (Exception e)
			{
				this.Response = e.ToString();
				OnPropertyChanged(() => this.Response);
			}
		}

		private void UpdateUrl()
		{
			var variables = this.SelectedEnvironment.Variables;
			var host = variables.Get("host", "na");
			this.Url = $"https://{host}/";
			OnPropertyChanged(() => this.Url);
		}

	}
}
