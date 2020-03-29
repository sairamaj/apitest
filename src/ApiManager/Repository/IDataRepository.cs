﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Repository
{
	interface IDataRepository
	{
		IEnumerable<ApiInfo> GetApiConfigurations();
		Task<ManagementCommandInfo> GetCommands(ApiInfo info);
		Task<IEnumerable<string>> GetVariables(ApiInfo info);
		void AddManagementInfo(Info info);
	}
}
