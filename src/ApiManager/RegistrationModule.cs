using System;
using System.IO;
using ApiManager.Model;
using ApiManager.Pipes;
using ApiManager.Repository;
using Autofac;
using Newtonsoft.Json;

namespace ApiManager
{
	internal class RegistrationModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			var settings = JsonConvert.DeserializeObject<Settings>(
				File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json")));
			settings.MakeAbsolultePaths();

			builder.RegisterInstance(settings).As<ISettings>();
			builder.RegisterType<CommandExecutor>().As<ICommandExecutor>().SingleInstance();
			builder.RegisterType<ResourceManager>().As<IResourceManager>().SingleInstance();
			builder.RegisterType<DataRepository>().As<IDataRepository>().SingleInstance();
			builder.RegisterType<MessageListener>().As<IMessageListener>();
			builder.RegisterType<ApiTestConsoleCommunicator>().As<IApiTestConsoleCommunicator>();
		}
	}
}