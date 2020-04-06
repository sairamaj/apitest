using System.Collections.Generic;
using System.Linq;
using ApiManager.Repository;

namespace ApiManager.Scripts.ViewModels
{
	class ScriptContainerViewModel
	{
		public ScriptContainerViewModel(IResourceManager resourceManager)
		{
			this.Scripts = resourceManager.GetScriptsData().Select(a => new ScriptViewModel(a));
		}

		public IEnumerable<ScriptViewModel> Scripts { get; set; }
	}
}
