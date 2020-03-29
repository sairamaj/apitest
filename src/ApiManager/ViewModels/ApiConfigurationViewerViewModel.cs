using System.Collections.Generic;
using System.IO;
using ApiManager.Model;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ApiConfigurationViewerViewModel : CoreViewModel
	{
		public ApiConfigurationViewerViewModel(ApiInfo apiInfo, IEnumerable<string> commands)
		{
			this.ApiInfo = apiInfo;
			this.ConfigurationData = File.ReadAllText(apiInfo.Configuration);
			this.Commands = commands;
		}

		public ApiInfo ApiInfo { get; }
		public string ConfigurationData { get; set; }
		public IEnumerable<string> Commands { get; set; }
	}
}
