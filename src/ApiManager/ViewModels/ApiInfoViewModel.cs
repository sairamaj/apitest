using System;
using System.IdentityModel.Tokens.Jwt;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using Newtonsoft.Json;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class ApiInfoViewModel : CoreViewModel
	{
		public ApiInfoViewModel(ApiInfo apiInfo)
		{
			this.ApiInfo = apiInfo;
			this.ShowJwtTokenCommand = new DelegateCommand(() =>
			{
				try
				{
					var jwtCode = ExtractJwtCode();
					var handler = new JwtSecurityTokenHandler();
					var token = handler.ReadJwtToken(jwtCode);
					MessageBox.Show($"apinfo :{token}");
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			});
		}

		public ApiInfo ApiInfo { get; set; }
		public ICommand ShowJwtTokenCommand { get; set; }

		private string ExtractJwtCode()
		{
			// Look in response first
			if (!string.IsNullOrWhiteSpace(this.ApiInfo.Response.Content))
			{
				// try to extract
				var token = JsonConvert.DeserializeObject<JwtToken>(this.ApiInfo.Response.Content);
				return token.Access_Token;
			}

			return string.Empty;
		}
	}
}
