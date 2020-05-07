using System.Collections.Generic;

namespace ApiManager.Model
{
	class ApiCommand
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string BaseUrl { get; set; }
		public IEnumerable<ApiRoute> Routes { get; set; }
	}
}
