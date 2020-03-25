using ApiManager.Model;
using ApiManager.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.Command;

namespace ApiManager.ViewModels
{
	class ApiInfoViewModel : InfoViewModel
	{
		public ApiInfoViewModel(ApiInfo apiInfo) : base(apiInfo)
		{
			this.ApiInfo = apiInfo;
			this.ShowJwtTokenCommand = new DelegateCommand(() =>
			{
				try
				{
					var viewModel = new JwtTokenViewModel(SerializeToken(apiInfo.JwtToken));
					var win = new JwtTokenWindow() { DataContext = viewModel };
					win.ShowDialog();
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			});
		}

		public ApiInfo ApiInfo { get; set; }
		public ICommand ShowJwtTokenCommand { get; set; }


		static string SerializeToken(String jwtToken)
		{
			var jwtHandler = new JwtSecurityTokenHandler();
			if (!jwtHandler.CanReadToken(jwtToken))
			{
				throw new Exception("The token doesn't seem to be in a proper JWT format.");
			}

			var token = jwtHandler.ReadJwtToken(jwtToken);
			var items = token.Header.ToDictionary(h => h.Key, h => h.Value);
			foreach (var c in token.Claims.GroupBy(c => c.Type))
			{
				if (c.Count() == 1)
				{
					items[c.Key] = c.FirstOrDefault()?.Value?.ToString();
				}
				else
				{
					var subItems = new List<string>();
					foreach (var v in c)
					{
						subItems.Add(v.Value);
					}

					items[c.Key] = subItems;
				}
			}

			return JsonConvert.SerializeObject(items, Formatting.Indented);
		}

		public static string ReadToken(string jwtInput)
		{
			var jwtHandler = new JwtSecurityTokenHandler();
			var jwtOutput = string.Empty;

			// Check Token Format
			if (!jwtHandler.CanReadToken(jwtInput)) throw new Exception("The token doesn't seem to be in a proper JWT format.");

			var token = jwtHandler.ReadJwtToken(jwtInput);

			// Re-serialize the Token Headers to just Key and Values
			var jwtHeader = JsonConvert.SerializeObject(token.Header.Select(h => new { h.Key, h.Value }));
			jwtOutput = $"{{\r\n\"Header\":\r\n{JToken.Parse(jwtHeader)},";

			// Re-serialize the Token Claims to just Type and Values
			var jwtPayload = JsonConvert.SerializeObject(token.Claims.Select(c => new { c.Type, c.Value }));
			jwtOutput += $"\r\n\"Payload\":\r\n{JToken.Parse(jwtPayload)}\r\n}}";

			// Output the whole thing to pretty Json object formatted.
			return JToken.Parse(jwtOutput).ToString(Formatting.Indented);
		}
	}
}
