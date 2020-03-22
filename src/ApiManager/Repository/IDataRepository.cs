using System.Collections.Generic;
using ApiManager.Model;

namespace ApiManager.Repository
{
	interface IDataRepository
	{
		IEnumerable<EnvironmentInfo> GetEnvironments();
		IEnumerable<string> GetCommands(EnvironmentInfo info);
	}
}
