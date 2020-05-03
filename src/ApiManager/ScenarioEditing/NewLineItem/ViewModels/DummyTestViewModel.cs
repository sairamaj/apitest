using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ScenarioEditing.NewLineItem.ViewModels
{
	class DummyTestViewModel : CoreViewModel
	{
		public DummyTestViewModel(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}
