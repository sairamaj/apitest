﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiManager.Model
{
	class ApiRoute
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Path { get; set; }
		public string BaseUrl { get; set; }
		public IDictionary<string, string> Headers { get; set; }
		public IDictionary<string, string> Body { get; set; }
		public bool IsDefault => this.Name == "_";
	}
}
