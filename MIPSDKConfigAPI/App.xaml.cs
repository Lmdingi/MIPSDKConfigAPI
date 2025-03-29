using System;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.SDK.UI.LoginDialog;

namespace MIPSDKConfigAPI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Guid integrationId = new Guid("0f417d49-9c45-4591-a62e-eb40d836aecc");
            string integrationName = "MIPSDKConfigAPI";
            string manufacturerName = "Your company name";
            string version = "1.0";

            VideoOS.Platform.SDK.Environment.Initialize();          // General initialize.  Always required
            VideoOS.Platform.SDK.UI.Environment.Initialize();       // Initialize UI references
            //VideoOS.Platform.SDK.Export.Environment.Initialize();	// Initialize export references
            //VideoOS.Platform.SDK.Media.Environment.Initialize();	// Initialize live streaming references

            bool connected = false;
            DialogLoginForm loginForm = new DialogLoginForm(new DialogLoginForm.SetLoginResultDelegate((b) => connected = b), integrationId, integrationName, version, manufacturerName);
            //loginForm.LoginLogoImage = MyOwnImage;				// Set own header image
            loginForm.ShowDialog();								// Show and complete the form and login to server

            if(connected)
            {
                // disable auto refresh when changed or updated detected
                VideoOS.Platform.SDK.Environment.Properties.EnableConfigurationRefresh = false;
                // set amout of waiting time to refresh if the above is true
                VideoOS.Platform.SDK.Environment.Properties.ConfigurationRefreshIntervalInMs = int.MaxValue;
                // disable constant checks or event handling in the background
                EnvironmentManager.Instance.EnableConfigurationChangedService = false;
            }

            if (!connected)
            {
                Current.Shutdown();
            }
        }
    }
}
