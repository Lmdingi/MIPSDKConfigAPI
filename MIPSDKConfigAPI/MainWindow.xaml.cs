using MIPSDKConfigAPI.Services;
using System.Collections.Generic;
using System;
using System.Windows;
using VideoOS.Platform.UI.Controls;
using VideoOS.ConfigurationApi.ClientService;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Forms;
using static VideoOS.Platform.Messaging.MessageId;
using VideoOS.Platform;


namespace MIPSDKConfigAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : VideoOSWindow
    {
        // props            
        public string StatusBarText { get; set; } = "http://localhost";

        // fields 
        private readonly ConfigApiService _configApiService;
        private readonly ConfigurationServiceClient _configServiceClient;
        private readonly VideoOSTreeViewItem _configItem;
        private readonly ItemDetailsService _itemDetailsService;
        // constructor
        public MainWindow()
        {
            InitializeComponent();
            _configApiService = new ConfigApiService();
            _configServiceClient = new ConfigurationServiceClient();
            _configItem = new VideoOSTreeViewItem();
            _itemDetailsService = new ItemDetailsService();
        }

        // methods
        private void ItemPicker_SelectedItemChanged(object sender, EventArgs e)
        {
            
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            //var a = _configServiceClient.GetItem("/");
            var server = _configApiService.GetItem("/");

            #region itemPicker
            //if (server != null)
            //{
            //var folders = new List<Item>();
            //itemPicker.Items = new List<Item>();

            //ServerId serverId = new ServerId(ServerId.CorporateManagementServerType, "top", 1, Guid.NewGuid());

            //Item item = new Item(serverId, Guid.Empty, Guid.NewGuid(), server.DisplayName, FolderType.No, Kind.Server);

            //folders.Add(item);

            //itemPicker.Items = folders;
            //}
            #endregion

            if (server != null)
            {
                _configItem.Data = server.DisplayName;
                _configItem.Tag = server;
                _configItem.IsExpanded = true;
                //_configItem.IconSource = 

                fillSelectedItemDetails(server);
                FillTVChildren(_configItem);

                rootTV.ItemsSource = new List<VideoOSTreeViewItem> { _configItem };
                statusLabel.Text = StatusBarText + server.Path;
            }

        }

        private void ItemPicker_SelectedItemChanged(object sender, RoutedEventArgs e)
        {

            if (rootTV.SelectedItem is VideoOSTreeViewItem selectedItem)
            {
                if (selectedItem.Children == null)
                {
                    FillTVChildren(selectedItem);
                }

                if (selectedItem.Tag is ConfigurationItem item)
                {
                    if (item != null)
                    {                        
                        fillSelectedItemDetails(item);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Selected item is not a valid ConfigurationItem.");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VideoOS.Platform.SDK.Environment.RemoveAllServers();
            Close();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (sender is VideoOSButtonPrimaryMedium button && button.Tag is ConfigurationItem item)
            {

            }
        }

        // helpers
        private void FillTVChildren(VideoOSTreeViewItem configItem)
        {
            var configItemTag = configItem.Tag as ConfigurationItem;

            var children = _configApiService.GetChildItems(configItemTag.Path);

            if (children != null)
            {
                List<VideoOSTreeViewItem> childrenTv = new List<VideoOSTreeViewItem>();

                foreach (var child in children)
                {
                    VideoOSTreeViewItem childTv = new VideoOSTreeViewItem
                    {
                        Data = child.DisplayName,
                        Tag = child,
                        IsExpanded = true,
                    };

                    childrenTv.Add(childTv);
                }

                configItem.Children = childrenTv.OrderBy(item => item.Data.ToString()).ToList();
            }
        }
        private VideoOSIconSourceBase GetIconForNode(string node)
        {
            string iconPath = "";

            //switch (node.ItemType)
            //{
            //    case "Folder":
            //        iconPath = "pack://application:,,,/Icons/folder.png";
            //        break;
            //    case "Camera":
            //        iconPath = "pack://application:,,,/Icons/camera.png";
            //        break;
            //    case "Server":
            //        iconPath = "pack://application:,,,/Icons/server.png";
            //        break;
            //    default:
            //        iconPath = "pack://application:,,,/Icons/default.png";
            //        break;
            //}

            return null;
        }

        private void fillSelectedItemDetails(ConfigurationItem item)
        {
            detailsPanel.Children.Clear();
            


            switch (item.ItemType)
            {
                case "System":
                    {
                        var details = _itemDetailsService.SetSystemDetails(item, Save_Click);

                        foreach (var detail in details)
                        {
                            detailsPanel.Children.Add(detail);
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            statusLabel.Text = StatusBarText + item.Path;
        }

        
    }
}
