using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Asserts.ViewModels;
using ApiManager.Repository;

namespace ApiManager.Variables.ViewModels
{
	
	class VariableGroupContainerViewModel
	{
		public VariableGroupContainerViewModel(IResourceManager resourceManager)
		{
			this.VariableGroups = resourceManager.GetVariableGroupData().Select(a => new VariableGroupViewModel(a));
		}

		public IEnumerable<VariableGroupViewModel> VariableGroups { get; set; }
	}

}
