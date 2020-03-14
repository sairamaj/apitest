using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class EnvironmentFolderViewModel : CommandTreeViewModel
	{
		public EnvironmentFolderViewModel(string name) : base(null, name, name)
		{
			this.Environments = new SafeObservableCollection<EnvironmentViewModel>();
			this.Environments.Add(new EnvironmentViewModel("Env_1"));
			this.Environments.Add(new EnvironmentViewModel("Env_2"));

			this.IsExpanded = true;
		}

		public ObservableCollection<EnvironmentViewModel> Environments { get; set; }

		protected override void LoadChildren()
		{
			foreach (var env in this.Environments)
			{
				this.Children.Add(env);
			}
		}
	}
}
