using System.Collections.Generic;
using ApiManager.Asserts.Model;
using ApiManager.Model;
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
	}
}
