using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using ApiManager.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wpf.Util.Core.Command;

namespace ApiManager.ViewModels
{
	class ApiInfoViewModel : InfoViewModel
	{
		public ApiInfoViewModel(ICommandExecutor executor, ApiRequest apiInfo) : base(executor, apiInfo)
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

			this.ViewAsHTMLCommand = new DelegateCommand(async () =>
		   {
			   string tempJsonFileName = string.Empty;
			   string tempHtmlFileName = string.Empty;
			   try
			   {
				   tempJsonFileName = FileHelper.WriteToTempFile(apiInfo.Response.Content, ".json");
				   tempHtmlFileName = FileHelper.GetTempFileName(".html");
				   await executor.ConvertJsonToHtml(tempJsonFileName, tempHtmlFileName);
				   if (File.Exists(tempHtmlFileName))
				   {
					   Process.Start(tempHtmlFileName);
				   }
			   }
			   catch (Exception e)
			   {
				   MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			   }
			   finally
			   {
				   FileHelper.DeleteIfExists(tempJsonFileName);
			   }
		   });

			this.SubmitRequestCommand = new DelegateCommand(async () =>
			{
				try
				{
					var response = await this.SubmitRequestAsync().ConfigureAwait(false);
					this.ApiInfo = response;
					OnPropertyChanged(() => this.ApiInfo);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.ToString());
				}
			});
		}

		public ApiRequest ApiInfo { get; set; }
		public ICommand ShowJwtTokenCommand { get; set; }
		public ICommand ViewAsHTMLCommand { get; set; }
		public ICommand SubmitRequestCommand { get; set; }

		public bool IsSuccess => this.ApiInfo.HttpCode >= 200 && this.ApiInfo.HttpCode <= 299;

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

		private async Task<ApiRequest> SubmitRequestAsync()
		{
			var request = new HttpRequestClient(this.ApiInfo);
			return await request.GetResponseAsync().ConfigureAwait(false);
		}
	}
}
