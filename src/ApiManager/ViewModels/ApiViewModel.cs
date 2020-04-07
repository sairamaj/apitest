using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.Views;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ApiViewModel : CommandTreeViewModel
	{
		private ICommandExecutor _executor;
		private IDataRepository _repository;
		public ApiViewModel(ApiInfo apiInfo, IDataRepository repository, ICommandExecutor executor) 
			: base(null, apiInfo.Name, apiInfo.Name)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
			this._repository = repository ?? throw new ArgumentNullException(nameof(repository)); 

			this.IsExpanded = true;
			this.ApiInfo = apiInfo;
			this.DataContext = this;
			this.RequestResponses = new SafeObservableCollection<InfoViewModel>();
			this.EditConfigFileCommand = new DelegateCommand(async () =>
			{
				await ShowConfigurationViewer().ConfigureAwait(false);
			});
		}

		public ApiInfo ApiInfo { get; set; }

		public ICommand RunCommand { get; set; }
		public ObservableCollection<InfoViewModel> RequestResponses { get; }

		public void Add(Info info)
		{
			var isError = false;
			if (info is ApiRequest)
			{
				this.RequestResponses.Add(new ApiInfoViewModel(this._executor, info as ApiRequest));
			}
			else if (info is ExtractVariableInfo)
			{
				this.RequestResponses.Add(new ExtractVariableViewModel(this._executor, info as ExtractVariableInfo));
			}
			else if (info is AssertInfo assertInfo)
			{
				this.RequestResponses.Add(new AssertInfoViewModel(this._executor, assertInfo));
				isError = !assertInfo.Success;
			}
			else if (info is ErrorInfo)
			{
				this.RequestResponses.Add(new ErrorInfoViewModel(info as ErrorInfo));
				isError = true;
			}
			else if (info is ApiExecuteInfo apiExecute)
			{
				this.RequestResponses.Add(new ApiExecuteInfoViewModel(apiExecute));
			}
			else if (info is JsScriptInfo jsScript)
			{
				this.RequestResponses.Add(new JsExecuteViewModel(jsScript));
				isError = jsScript.IsError;
			}

			if (isError)
			{
				var apiInfoViewModel = this.RequestResponses.OfType<ApiExecuteInfoViewModel>().FirstOrDefault();
				apiInfoViewModel?.SetStatusToFail();
			}
		}

		public ICommand EditConfigFileCommand { get; }

		public async Task ShowConfigurationViewer()
		{
			try
			{
				var commands = await this._repository.GetCommands(this.ApiInfo).ConfigureAwait(true);
				var viewer = new ApiConfigurationViewer() { DataContext = new ApiConfigurationViewerViewModel(this.ApiInfo, commands) };
				viewer.ShowDialog();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
