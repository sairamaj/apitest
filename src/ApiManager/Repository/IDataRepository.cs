using System.Collections.Generic;
using System.Threading.Tasks;
using ApiManager.Asserts.Model;
using ApiManager.Model;

namespace ApiManager.Repository
{
	interface IDataRepository
	{
		IEnumerable<ApiInfo> GetApiConfigurations();
		Task<ApiCommandInfo> GetCommands(ApiInfo info);
		Task<IEnumerable<string>> GetVariables(ApiInfo info);
		Task<IEnumerable<HelpCommand>> GetHelpCommands();
		void AddManagementInfo(Info info);
		Scenario CopyScenario(Scenario scenario);
	}
}
