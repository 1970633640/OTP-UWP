using OTP_UWP.Functions;
using OtpNet;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            Debug.WriteLine("add steam start!");
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Back_Click(sender,e);
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
                this.Frame.BackStack.Remove(Frame.BackStack[Frame.BackStack.Count - 1]);
                this.Frame.GoBack();
            }
        }
        private async void Done_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            if (otp_secret.Text == "")
            {
                err_text.Text = resourceLoader.GetString("err_empty_steam_secret");
            }
            else
            {
                try
                {
                    Base32Encoding.ToBytes(otp_secret.Text);
                    await SqlAccess.Add_Item(1, "Steam",otp_label.Text, otp_secret.Text, 0, 5, 30,1,"steam");
                    MainPage.mainPage.init_data();
                    back();
                }
                catch
                {
                    err_text.Text = resourceLoader.GetString("err_wrong_steam_secret");
                }
            }
        }
    }
}
