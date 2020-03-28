using System.Collections.Generic;
using ApiManager.Model;

namespace ApiManager.Repository
{
	interface IDataRepository
	{
		IEnumerable<ApiInfo> GetApiConfigurations();
		IEnumerable<string> GetCommands(ApiInfo info);
	}
}
