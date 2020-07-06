using OTP_UWP.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class AddSteam : Page
    {
        public AddSteam()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Back_Click(sender,e);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.BackStack.Remove(Frame.BackStack[Frame.BackStack.Count - 1]);
            this.Frame.GoBack();
        }

        private async void Done_Click(object sender, RoutedEventArgs e)
        {
            await SqlAccess.Add_Item(1, "Steam", "Steam", otp_secret.Text, 0, 5, 30);
            Back_Click(sender, e);
        }
    }
}
