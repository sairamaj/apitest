﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ApiManager.Model;
using ApiManager.Repository;
using Wpf.Util.Core;
using Wpf.Util.Core.Command;
using Wpf.Util.Core.ViewModels;

namespace ApiManager.ViewModels
{
	class EnvironmentViewModel : CommandTreeViewModel
	{
		private IApiExecutor _executor;
		public EnvironmentViewModel(EnvironmentInfo env, IApiExecutor executor) : base(null, env.Name, env.Name)
		{
			this._executor = executor ?? throw new ArgumentNullException(nameof(executor));
			this.IsExpanded = true;
			this.EnvironmentInfo = env;
			this.DataContext = this;
			this.RequestResponses = new SafeObservableCollection<InfoViewModel>();
			this.EditConfigFileCommand = new DelegateCommand(() =>
			{
				try
				{
					Process.Start("notepad", this.EnvironmentInfo.Configuration);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			});

		}

		public EnvironmentInfo EnvironmentInfo { get; set; }

		public ICommand RunCommand { get; set; }
		public ObservableCollection<InfoViewModel> RequestResponses { get; }

		public void Add(Info info)
		{
			if (info is ApiInfo)
			{

				this.RequestResponses.Add(new ApiInfoViewModel(this._executor, info as ApiInfo));
			}
			else if (info is ExtractVariableInfo)
			{
				this.RequestResponses.Add(new ExtractVariableViewModel(this._executor, info as ExtractVariableInfo));
			}
			else if (info is AssertInfo)
			{
				this.RequestResponses.Add(new AssertInfoViewModel(this._executor, info as AssertInfo));
			}
		}

		public ICommand EditConfigFileCommand { get; }
	}
}
