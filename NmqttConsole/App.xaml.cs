using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using NmqttConsole.Presenters;

namespace NmqttConsole
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            ShellPresenter shell = new ShellPresenter();
            shell.Show();
            shell.View.Closed += (sender, closedEventArgs) => this.Shutdown();
        }
    }
}
