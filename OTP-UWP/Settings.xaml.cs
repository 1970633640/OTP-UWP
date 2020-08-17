using Newtonsoft.Json;
using OTP_UWP.Functions;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace OTP_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            back();
        }
        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            back();
        }

        private void back()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async void export_clipboard_Click(object sender, RoutedEventArgs e)
        {
            var x = await SqlAccess.Get_All_Item();
            var s=JsonConvert.SerializeObject(x);
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(s);
            Clipboard.SetContent(dataPackage);
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["CLICK_COPY"] = toggle1.IsOn;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("CLICK_COPY"))
                toggle1.IsOn = (bool)roamingSettings.Values["CLICK_COPY"];
            else
                toggle1.IsOn = true; 
        }
    }
}
