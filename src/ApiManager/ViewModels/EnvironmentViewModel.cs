using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class EnvironmentViewModel : CommandTreeViewModel
	{
		public EnvironmentViewModel(string name) : base(null, name, name)
		{
			this.IsExpanded = true;
		}
	}
}
