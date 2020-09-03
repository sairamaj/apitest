using Wpf.Util.Core.ViewModels;

namespace ApiManager.NewRequest.ViewModel
{
	class VariableNameValueViewModel : CoreViewModel
	{
		private string _value;
		public VariableNameValueViewModel(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; }
		public string Value
		{
			get => this._value; set
			{
				this._value = value;
				OnPropertyChanged(() => this.Value);
			}
		}
	}
}
