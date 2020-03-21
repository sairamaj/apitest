using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	internal class JwtTokenViewModel : CoreViewModel
	{
		public JwtTokenViewModel(string token)
		{
			this.Token = token;
		}

		public string Token { get; set; }
	}
}
