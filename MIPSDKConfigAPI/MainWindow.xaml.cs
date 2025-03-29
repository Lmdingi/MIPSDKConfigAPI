using MIPSDKConfigAPI.Services;
using System.Collections.Generic;
using System;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.Login;
using VideoOS.ConfigurationApi.ClientService;
using System.Drawing;
using System.IO;

namespace MIPSDKConfigAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : VideoOSWindow
    {
        // props            

        // fields 
        private readonly ConfigApiService _configApiService;
        // constructor
        public MainWindow()
        {
            InitializeComponent();
            _configApiService = new ConfigApiService();
        }

        // methods
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VideoOS.Platform.SDK.Environment.RemoveAllServers();
            Close();
        }

        // helpers
    }
}
