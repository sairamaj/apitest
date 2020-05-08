using System.Collections.Generic;
using ApiManager.Asserts.Model;
using ApiManager.Model;
using ApiManager.Resources.Model;
using ApiManager.Scripts.Models;
using Environment = ApiManager.Model.Environment;

namespace ApiManager.Repository
{
	interface IResourceManager
	{
		IDictionary<string, string> GetSystemVariables();
		IDictionary<string, string> Get(ApiInfo apiInfo);
		IDictionary<string, string> Get(Environment environment);
		IEnumerable<AssertData> GetAssertData();
		IEnumerable<ScriptData> GetScriptsData();
		IEnumerable<ResourceData> GetResources(string method);
		string GetResourcePath(string method);
		IEnumerable<VariableGroupData> GetVariableGroupData();
		string SaveApiRequestPayload(string content, string name, string method);
	}
}
