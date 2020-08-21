using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace OTP_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class About : Page
    {
        public About()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            quota.Text="Roaming Storage Quota: "+ Windows.Storage.ApplicationData.Current.RoamingStorageQuota.ToString()+" KB";
            
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
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager.GetForCurrentView().BackRequested -= BackRequested;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Loaded!");
        }
    }
}
