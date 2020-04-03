using System;
using System.Collections.Generic;
using System.IO;
using ApiManager.Model;

namespace ApiManager.Repository
{
	internal class VariableManager : IVariableManager
	{
		private readonly ISettings _settings;

		public VariableManager(ISettings settings)
		{
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}
		
		public IDictionary<string, string> GetSystemVariables()
		{
			return GetVariables(Path.Combine(this._settings.ConfigurationPath, "variables.var"));
		}

		public IDictionary<string, string> Get(ApiInfo apiInfo)
		{
			if (apiInfo == null)
			{
				return new Dictionary<string, string>();
			}

			return GetVariables(Path.Combine(apiInfo.Path, "variables.var"));
		}

		public IDictionary<string, string> Get(Model.Environment environment)
		{
			if (environment == null)
			{
				return new Dictionary<string, string>();
			}

			return GetVariables(environment.FileName);
		}

		private IDictionary<string, string> GetVariables(string fileName)
		{
			var variables = new Dictionary<string, string>();
			if (File.Exists(fileName))
			{
				foreach (var line in File.ReadAllLines(fileName))
				{
					if (line.StartsWith("#", StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}
					var parts = line.Split('=');
					if (parts.Length > 1)
					{
						variables[parts[0]] = parts[1];
					}
					else
					{
						variables[parts[0]] = string.Empty;
					}
				}
			}

			return variables;
		}
	}
}
