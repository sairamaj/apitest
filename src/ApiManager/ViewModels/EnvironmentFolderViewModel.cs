using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Model;
using ApiManager.Pipes;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class EnvironmentFolderViewModel : CommandTreeViewModel
	{
		public EnvironmentFolderViewModel(
			string name, 
			IEnumerable<EnvironmentInfo> environments, 
			IApiExecutor executor) : base(null, name, name)
		{
			this.Environments  = environments.Select(e => new EnvironmentViewModel(e, executor)).ToList();
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

		public void AddApiInfo(string name, ApiInfo apiInfo)
		{
			//var env = this.Environments.FirstOrDefault(e => e.Name == name);
			var env = this.Environments.FirstOrDefault();
			if (env == null)
			{
				return;
			}

			env.AddApiInfo(apiInfo);
		}

	}
}
