using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiManager.Model;

namespace ApiManager.Repository
{
	class DataRepository : IDataRepository
	{
		public IDictionary<string, IEnumerable<EnvironmentInfo>> GetEnvironments()
		{
			return new Dictionary<string, IEnumerable<EnvironmentInfo>>()
			{
				{ 
					"Apigee", new List<EnvironmentInfo>{
					new EnvironmentInfo("SairamaJ","apigee.json")
				}
				}
			};
		}
	}
}
