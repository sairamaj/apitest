using System.Collections.Generic;
using System.Linq;
using ApiManager.Model;
using Microsoft.OpenApi.Models;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ApiConfigEditing.ViewModels
{
	class EditorViewModel : CoreViewModel
	{
		public EditorViewModel(OpenApiDocument apiDocument, ApiCommandInfo apiCommandInfo)
		{
			this.Paths = apiDocument.Paths.Select(kv => new PathViewModel(kv.Key, kv.Value));
			this.ApiCommands = apiCommandInfo.ApiCommands.Select(api => new ApiCommandViewModel(api));
		}

		public IEnumerable<PathViewModel> Paths { get; set; }
		public IEnumerable<ApiCommandViewModel> ApiCommands { get; set; }
	}
}
