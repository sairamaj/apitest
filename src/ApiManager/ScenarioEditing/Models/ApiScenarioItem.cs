using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Wpf.Util.Core.Command;

namespace ApiManager.ScenarioEditing.Models
{
	class ApiScenarioItem : ScenarioLineItem
	{
		string _command;
		public ApiScenarioItem(string command) : base("api", command)
		{
			this.Command = command;
			this.Parse(command);
		}

		public string Command
		{
			get
			{
				return this._command;
			}
			set
			{
				this.Parse(value);
				this._command = value;
				OnPropertyChanged(() => this.ApiName);
				OnPropertyChanged(() => this.Method);
				OnPropertyChanged(() => this.PayloadFileName);
			}
		}

		public string ApiName { get; set; }
		public string Method { get; set; }
		public string PayloadFileName { get; set; }
		public IEnumerable<string> Apis { get; }

		public override string GetCommand()
		{
			return this.IsCommented ? $"# {this._command}" : this._command;
		}

		private void Parse(string command)
		{
			var parts = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			this.ApiName = parts[0];
			this.Method = parts.Length > 1 ? parts[1]: null;
			this.PayloadFileName = parts.Length > 2 ? parts[2] : null;
		}
	}
}
