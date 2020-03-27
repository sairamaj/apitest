using System.Collections.Generic;
using ApiManager.Model;

namespace ApiManager.Repository
{
	interface IDataRepository
	{
		IEnumerable<ApiInfo> GetEnvironments();
		IEnumerable<string> GetCommands(ApiInfo info);
	}
}
