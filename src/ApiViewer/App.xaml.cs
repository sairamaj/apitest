using System;
using System.Diagnostics;
using System.Windows;
using ApiViewer.Pipes;
using ApiViewer.Properties;
using ApiViewer.ViewModel;
using Autofac;
using Wpf.Util.Core.Extensions;
using Wpf.Util.Core.Registration;

namespace ApiViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += (s, ex) =>
            {
                Trace.WriteLine(ex.Exception.GetExceptionDetails());
                MessageBox.Show(ex.Exception.GetExceptionDetails());
                System.Environment.Exit(-1);
            };

            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<MessageListener>().As<IMessageListener>();
                var serviceLocator = ServiceLocatorFactory.Create(builder);
                var win = new MainWindow()
                {
                    DataContext = new MainViewModel(serviceLocator.Resolve<IMessageListener>())
                };

                win.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.GetExceptionDetails());
            }
        }
    }
}
