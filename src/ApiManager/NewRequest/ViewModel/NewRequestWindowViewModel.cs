using System.Linq;

namespace ApiManager.NewRequest.ViewModel
{
	class NewRequestWindowViewModel
	{
		public NewRequestWindowViewModel()
		{
			this.SelectedMethod = HttpMethods.First();
		}

		public string[] HttpMethods => new string[]{ "GET","POST","PUT","PATCH","DELETE"};
		public string SelectedMethod { get; set; }
	}
}
