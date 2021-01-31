using System;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace IsTheMicInUse
{
    public partial class App : Application
    {
        private readonly Mutex _onlyOneAppInstanceIsRunningMutex = new Mutex(true, Assembly.GetExecutingAssembly().FullName);

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (_onlyOneAppInstanceIsRunningMutex.WaitOne(TimeSpan.Zero))
            {
                Poller poller = new Poller();
                new Thread(poller.PollMicrophone).Start();
            }
            else
            {
                MessageBox.Show("Already running.", Assembly.GetExecutingAssembly().GetName().Name, MessageBoxButton.OK, MessageBoxImage.Information);
                Shutdown();
            }
        }
    }
}
