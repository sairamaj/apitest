using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class EnvironmentFolderViewModel : CommandTreeViewModel
	{
		public EnvironmentFolderViewModel(string name, IEnumerable<EnvironmentInfo> environments, IApiExecutor executor) : base(null, name, name)
		{
			this.Environments  = environments.Select(e => new EnvironmentViewModel(e.Name, executor));
			this.IsExpanded = true;
		}

		public IEnumerable<EnvironmentViewModel> Environments { get; set; }

		protected override void LoadChildren()
		{
			foreach (var env in this.Environments)
			{
				this.Children.Add(env);
			}
		}
	}
}
