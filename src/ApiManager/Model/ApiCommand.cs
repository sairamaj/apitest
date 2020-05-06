using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiManager.Model
{
	class ApiCommand
	{
		public string Name { get; set; }
		public string BaseUrl { get; set; }
		public IEnumerable<ApiRoute> Routes { get; set; }
	}
}
