using System.Collections.Generic;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Repository
{
	interface IApiExecutor
	{
		Task<string> StartAsync(TestData testData);
		Task<string> OpenCommandPromptAsync(TestData testData);
		Task<string> GetCommands(string configFile);
		Task<string> ConvertJsonToHtml(string jsonFile);
	}
}
