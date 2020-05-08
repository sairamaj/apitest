using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiManager.Asserts.Model;
using ApiManager.Model;
using ApiManager.Resources.Model;
using ApiManager.Scripts.Models;

namespace ApiManager.Repository
{
	internal class ResourceManager : IResourceManager
	{
		private readonly ISettings _settings;

		public ResourceManager(ISettings settings)
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

		public IEnumerable<AssertData> GetAssertData()
		{
			var assertsDirectory = Path.Combine(this._settings.ResourcesPath, "Asserts");
			if (Directory.Exists(assertsDirectory))
			{
				return Directory.GetFiles(assertsDirectory)
					.Select(f => new AssertData(Path.GetFileNameWithoutExtension(f), f));
			}

			return new List<AssertData>();
		}

		public IEnumerable<VariableGroupData> GetVariableGroupData()
		{
			var varaibleGroupDirectory = Path.Combine(this._settings.ResourcesPath, "Variables");
			if (Directory.Exists(varaibleGroupDirectory))
			{
				return Directory.GetFiles(varaibleGroupDirectory)
					.Select(f => new VariableGroupData(Path.GetFileNameWithoutExtension(f), f));
			}

			return new List<VariableGroupData>();
		}

		public IEnumerable<ScriptData> GetScriptsData()
		{
			var path = Path.Combine(this._settings.ResourcesPath, "Scripts");
			if (Directory.Exists(path))
			{
				return Directory.GetFiles(path)
					.Select(f => new ScriptData(Path.GetFileNameWithoutExtension(f), f));
			}

			return new List<ScriptData>();
		}

		public IEnumerable<ResourceData> GetResources(string method)
		{
			var path = Path.Combine(this._settings.ResourcesPath, method);
			if (Directory.Exists(path))
			{
				return Directory.GetFiles(path)
					.Select(f => new ResourceData(Path.GetFileNameWithoutExtension(f), f));
			}

			return new List<ResourceData>();
		}

		public string SaveApiRequestPayload(string content, string name, string method)
		{
			var fileName = Path.Combine(this._settings.ResourcesPath, method, name);
			fileName = Path.ChangeExtension(fileName, "json");
			File.WriteAllText(fileName, content);
			return fileName;
		}
	}
}
