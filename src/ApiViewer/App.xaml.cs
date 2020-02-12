using System;
using System.Diagnostics;
using System.Windows;
using ApiViewer.Pipes;
using ApiViewer.ViewModel;
using Wpf.Util.Core.Extensions;

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
                var win = new MainWindow()
                {
                    DataContext = new MainViewModel(new MessageListener())
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
