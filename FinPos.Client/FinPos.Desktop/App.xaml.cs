using FinPos.Client.Controllers;
using FinPos.Client.Views;
using FinPos.DomainContracts.DataContracts;
using FinPos.Utility.CommonMethods;
using System;
using System.Linq;
using System.Windows;

namespace FinPos.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    ///

    public partial class App : Application
    {
        public SystemConfigurationController systemConfigurationController = new SystemConfigurationController();
        public App()
        {
            SystemConfigurationModel systemConfigurations = systemConfigurationController.GetSystemConfiguration().FirstOrDefault();
            Settings.FinalYearStartDate = systemConfigurations?.FinalYearStartDate;
            Settings.FinalYearEndDate = systemConfigurations?.FinalYearEndDate;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            throw new Exception("Test");
          var systemConfigurations=  systemConfigurationController.GetSystemConfiguration();
            // Main mainWind = Application.Current.MainWindow as Main;
            //add some bootstrap or startup logic 
            var identity = "aa";
            if (identity == null)
            {
                Login login = new Login();
                login.Show();
            }
            else
            {
                // dynamic sobj = new Main() as Window;
                Application curApp = Application.Current;
                // curApp.StartupUri = new Uri("SecondWindow.xaml", UriKind.RelativeOrAbsolute);
                curApp.StartupUri = new Uri("Views/Main.xaml", UriKind.Relative);
                curApp.MainWindow = new Main() as Window;
                //sobj.MainMdiContainer.Children.Add(new MdiChild()
                //{
                //    MinWidth = 1090,
                //    MinHeight = 500,
                //    Resizable = false,
                //    WindowState = WindowState.Maximized,
                //    Content = new Company()
                //});
                //sobj.Show();
            }
        }


    }
}

