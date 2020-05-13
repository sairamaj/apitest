using Microsoft.OpenApi.Models;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ApiConfigEditing.ViewModels
{
	class PathViewModel : CommandTreeViewModel
	{
		public PathViewModel(string path, OpenApiPathItem pathItem) : base(null, path, path)
		{
			this.IsExpanded = true;
		}
	}
}
