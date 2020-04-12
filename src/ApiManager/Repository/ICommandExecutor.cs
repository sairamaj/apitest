using System.Collections.Generic;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Repository
{
	interface ICommandExecutor
	{
		Task<string> StartAsync(TestData testData);
		Task<string> OpenCommandPromptAsync(TestData testData);
		Task<string> GetApiCommands(ApiInfo info);
		Task<string> GetApiVariables(ApiInfo info);
		Task<string> GetHelpCommands();
		Task<string> ConvertJsonToHtml(string jsonFile, string outHtmlFile);
		Task<string> SubmitHttpRequest(string requestFile, string id);
	}
}
