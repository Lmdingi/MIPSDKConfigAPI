using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.UI.Controls;

namespace MIPSDKConfigAPI.Services
{
    public class ItemDetailsService
    {
        // props

        // fields

        // constructors
        public ItemDetailsService()
        {
            
        }

        // methods
        public List<UIElement> SetSystemDetails(ConfigurationItem item, RoutedEventHandler Save_Click)
        {
            // note: make dynamic details
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            VideoOSTextBlockBodySmall label = new VideoOSTextBlockBodySmall();
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Width = 100;
            label.Text = item.Properties[0].Key + ":";

            VideoOSTextBlockBodySmall value = new VideoOSTextBlockBodySmall();
            value.Text = item.Properties[0].Value;

            stackPanel.Children.Add(label);
            stackPanel.Children.Add(value);
            // == //

            StackPanel stackPanel2 = new StackPanel();
            stackPanel2.Orientation = Orientation.Horizontal;

            VideoOSTextBlockBodySmall label2 = new VideoOSTextBlockBodySmall();
            label2.VerticalAlignment = VerticalAlignment.Center;
            label2.Width = 100;
            label2.Text = item.Properties[1].Key + ":";

            VideoOSTextBoxSmall value2 = new VideoOSTextBoxSmall();
            value2.Text = item.Properties[1].Value;

            stackPanel2.Children.Add(label2);
            stackPanel2.Children.Add(value2);
            // ================================== //

            List<UIElement> uiControls = new List<UIElement>
            {
                SetTitle(item),
                stackPanel,
                stackPanel2,
                SaveButton(Save_Click, item)
            };

            return uiControls;
        }
        private VideoOSTextBlockTitle SetTitle(ConfigurationItem item)
        {
            VideoOSTextBlockTitle title = new VideoOSTextBlockTitle();
            title.Text = item.DisplayName;
            return title;
        }

        // helpers
        private VideoOSButtonPrimaryMedium SaveButton(RoutedEventHandler Save_Click, ConfigurationItem item)
        {
            VideoOSButtonPrimaryMedium save = new VideoOSButtonPrimaryMedium();
            save.Content = "Save";
            save.VerticalAlignment = VerticalAlignment.Bottom;
            save.HorizontalAlignment = HorizontalAlignment.Right;
            save.Click += Save_Click;
            save.Margin = new Thickness(0, 10, 10, 0);

            save.Tag = item;

            return save;
        }
    }
}
