using System;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Common
{
	class TreeItemSelectionChangedEventArg : EventArgs
	{
		public TreeItemSelectionChangedEventArg(TreeViewItemViewModel treeViewItemViewModel, bool isSelected)
		{
			TreeViewItemViewModel = treeViewItemViewModel;
			IsSelected = isSelected;
		}

		public TreeViewItemViewModel TreeViewItemViewModel { get; }
		public bool IsSelected { get; }
	}
}
