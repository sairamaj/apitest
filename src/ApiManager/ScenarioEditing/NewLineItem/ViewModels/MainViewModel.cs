using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ApiManager.Model;
using Wpf.Util.Core;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	internal class MainViewModel : CoreViewModel
	{
		public MainViewModel(
			IEnumerable<BangCommandInfo> bangCommands,
			ApiCommandInfo apiCommandInfo)
		{
			this.RootCommands = new List<CommandTreeViewModel>()
			{
				new ApiInfoContainerViewModel(apiCommandInfo),
				new BangContainerCommandInfoViewModel(bangCommands),
				new FunctionInfoViewModel("functions", "functions")
			};

			this.Editor = new EditorViewModel();
			this.DeleteContactList = new SafeObservableCollection<DummyTestViewModel>();
			this.DeleteContactList.Add(new DummyTestViewModel("item1"));
			this.DeleteContactList.Add(new DummyTestViewModel("item2"));
			this.DeleteContactList.Add(new DummyTestViewModel("item3"));
		}

		public IEnumerable<CommandTreeViewModel> RootCommands { get; }
		public ObservableCollection<DummyTestViewModel> ListItems { get; }
		public EditorViewModel Editor { get; }

		public override void OnDrop(Point point, IDataObject data)
		{
			MessageBox.Show("on drop!");
		}

		private ObservableCollection<DummyTestViewModel> _deleteContactList;
		public ObservableCollection<DummyTestViewModel> DeleteContactList
		{
			get
			{
				return _deleteContactList;
			}
			set
			{
				_deleteContactList = value;
				OnPropertyChanged(() => DeleteContactList);
			}
		}

	}
}
