using ApiManager.Views;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow 
	{
		public MainWindow()
		{
			InitializeComponent();
			//this.EnvironmentView.SelectionChangedEvent += (s, e) =>
			//{
			//	if (!(e.SelectedItem is CommandTreeViewModel viewModel))
			//	{
			//		return;
			//	}

			//	var ctrl = new RunDetailsView { DataContext = viewModel.DataContext };
			//	this.DetailViewContainer.ShowView(ctrl);
			//};
		}
	}
}
