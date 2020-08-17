using OTP_UWP.Functions;
using OtpNet;
using System;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace OTP_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Add : Page
    {
        private int type=0;
        private string logo="";
        private string path;
        public Add()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            otp_period.SelectedIndex = 1;
            otp_digits.SelectedIndex = 2;
            otp_algorithm.SelectedIndex = 0;
            no_text.Visibility = Visibility.Visible;
            image.Visibility = Visibility.Collapsed;
            emoji_text.Visibility= Visibility.Collapsed;
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
        private void Add_Steam_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddSteam));
        }

        private async void image_change_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            ContentDialog chooseLogoDialog = new ChooseLogoDialog
            {
                Title = resourceLoader.GetString("image_choose"),
                PrimaryButtonText =  resourceLoader.GetString("ok"),
                SecondaryButtonText = resourceLoader.GetString("cancel2"),
            };

            ContentDialogResult result = await chooseLogoDialog.ShowAsync();
            if(result== ContentDialogResult.Primary)
            {
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values["type"] != null)
                {
                    type = (int)localSettings.Values["type"];
                }
                logo = localSettings.Values["logo"] as string;
                if (type == 0)
                {
                    no_text.Visibility = Visibility.Visible;
                    image.Visibility = Visibility.Collapsed;
                    emoji_text.Visibility = Visibility.Collapsed;
                }
                else if (type == 1)
                {
                    no_text.Visibility = Visibility.Collapsed;
                    image.Visibility = Visibility.Visible;
                    emoji_text.Visibility = Visibility.Collapsed;
                    image.Source = new SvgImageSource(new Uri("ms-appx:///Assets/Logos/" + logo + ".svg"));
    }
                else
                {
                    no_text.Visibility = Visibility.Collapsed;
                    image.Visibility = Visibility.Collapsed;
                    emoji_text.Visibility = Visibility.Visible;
                    emoji_text.Text = logo;
                }
            }
        }

        private async void Done_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            if (otp_secret.Text == "")
            {
                err_text.Text = resourceLoader.GetString("err_empty_secret");
            }
            else
            {
                try
                {
                    Base32Encoding.ToBytes(otp_secret.Text.Trim());
                    await SqlAccess.Add_Item(0, otp_issuer.Text, otp_label.Text, otp_secret.Text, otp_algorithm.SelectedIndex, (int)otp_digits.SelectedValue, (int)otp_period.SelectedValue, type, logo);
                    MainPage.mainPage.init_data();
                    back();
                }
                catch
                {
                    err_text.Text = resourceLoader.GetString("err_wrong_secret");
                }
            }
        }
    }
}
