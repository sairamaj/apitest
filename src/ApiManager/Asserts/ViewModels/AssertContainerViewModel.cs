using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Repository;

namespace ApiManager.Asserts.ViewModels
{
	class AssertContainerViewModel
	{
		public AssertContainerViewModel(IResourceManager resourceManager)
		{
			this.Asserts = resourceManager.GetAssertData().Select(a => new AssertViewModel(a));
		}

		public IEnumerable<AssertViewModel> Asserts { get; set; }
	}
}
