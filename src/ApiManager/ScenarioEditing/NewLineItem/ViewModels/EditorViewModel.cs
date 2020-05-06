using System.Windows;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	class EditorViewModel : CoreViewModel
	{
		public EditorViewModel()
		{

		}

		public override void OnDrop(Point point, IDataObject data)
		{
			MessageBox.Show("on drop");
			// MessageBox.Show(data.GetData(typeof(string).ToString());
		}
	}
}
