using System.Collections.Generic;
using ApiManager.Model;
using Environment = ApiManager.Model.Environment;

namespace ApiManager.Repository
{
	interface IVariableManager
	{
		IDictionary<string, string> GetSystemVariables();
		IDictionary<string, string> Get(ApiInfo apiInfo);
		IDictionary<string, string> Get(Environment environment);
	}
}
