using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;
using ApiEnvironment = ApiManager.Model.Environment;

namespace ApiManager.ViewModels
{
	class EnvironmentViewModel : CoreViewModel
	{
		public EnvironmentViewModel(ApiInfo apiInfo, ApiEnvironment environment, IDataRepository repository)
		{
			this.Environment = environment;
			this.FileName = environment.FileName;
			this.Name = Path.GetFileNameWithoutExtension(environment.Name);
			this.EditCommandFileCommand = new DelegateCommand(async () =>
		   {
			   try
			   {
				   //var variables = await repository.GetVariables(apiInfo).ConfigureAwait(true);
				   //var msg = string.Join("\r\n", variables);
				   //MessageBox.Show(msg);
				   Process.Start("notepad", this.FileName);
			   }
			   catch (Exception e)
			   {
				   MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			   }
		   });
		}

		public string Name { get; }
		public string FileName { get; }
		public ICommand EditCommandFileCommand { get; }
		public ApiEnvironment Environment { get; }
		public IDictionary<string, string> Variables
		{
			get
			{
				var variables = new Dictionary<string, string>();
				if (!File.Exists(this.FileName))
				{
					return variables;
				}

				foreach (var line in File.ReadAllLines(this.FileName))
				{
					var parts = line.Split('=');
					if (parts.Length > 1)
					{
						var key = parts.First();
						variables[key] = line.Substring(key.Length + 1);
					}
				}

				return variables;
			}
		}
	}
}
