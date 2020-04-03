namespace ApiManager.Variables.ViewModels
{
	class VariableViewModel
	{
		public VariableViewModel(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; }
		public string Value { get; }
	}
}
