using System;
using System.Diagnostics;
using System.Windows.Input;
using ApiManager.Common;
using ApiManager.Resources.Model;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Resources.ViewModels
{
	class ResourceTreeViewModel : CommandTreeViewModel
	{
		public event EventHandler<TreeItemSelectionChangedEventArg> SelectionChanged;
		public ResourceTreeViewModel(ResourceTreeViewModel parent, ResourceData resource) : base(parent, resource.Name, resource.FileName)
		{
			this.Resource = resource;
			this.RelvealInExplorerCommand = new DelegateCommand(() =>
		   {
			   Process.Start(resource.ContainerPath);
		   });
		}

		public ResourceData Resource { get; }

		public override bool IsSelected
		{
			get => base.IsSelected;
			set
			{
				base.IsSelected = value;
				if (SelectionChanged != null)
				{
					var eventArg = new TreeItemSelectionChangedEventArg(this, value);
					SelectionChanged(this, eventArg);
					//PropagateToParent(eventArg);
				}
			}
		}

		protected void PropagateToParent(TreeItemSelectionChangedEventArg eventArg)
		{
			this.SelectionChanged?.Invoke(this, eventArg);
		}

		public ICommand RelvealInExplorerCommand { get; }
	}
}
