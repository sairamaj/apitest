using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.ViewModels;
using Wpf.Util.Core.Views;

namespace ApiManager.ScenarioEditing.NewLineItem.Views
{
	/// <summary>
	/// Interaction logic for CommandView.xaml
	/// </summary>
	public partial class CommandView : CoreUserControl
	{
		public CommandView()
		{
			InitializeComponent();
		}

        /// <summary>
        /// On mouse move.
        /// </summary>
        /// <param name="e">
        /// MouseEvent arguments.
        /// </param>
        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var viewModel = this.DataContext as CoreViewModel;
                var data = viewModel?.GetDragData();
                if (data == null)
                {
                    return;
                }

                DragDrop.DoDragDrop(this, data, DragDropEffects.Copy);
            }
        }
    }
}
