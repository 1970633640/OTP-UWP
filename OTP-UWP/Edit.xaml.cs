using OTP_UWP.DataClass;
using OTP_UWP.Functions;
using OtpNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace OTP_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Edit : Page
    {
        int id = 0;
        OtpItem otp;
        private int type = 0;
        private string logo = "";
        private string path;

        public Edit()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = resourceLoader.GetString("Delete_permanently"),
                Content = "\n",
                PrimaryButtonText = resourceLoader.GetString("Delete2"),
                CloseButtonText = resourceLoader.GetString("cancel2")
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            // Delete the file if the user clicked the primary button.
            /// Otherwise, do nothing.
            if (result == ContentDialogResult.Primary)
            {
                SqlAccess.Delete_Item(id);
                MainPage.mainPage.init_data();
                back();
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }
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
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is int)
            {
                id = (int)e.Parameter;
                otp =await SqlAccess.Get_Item(id);
                init_data();
            }
            base.OnNavigatedTo(e);
        }
        private void init_data()
        {
            otp_issuer.Text = otp.Issuer;
            otp_label.Text = otp.Name;
            otp_secret.Text = otp.Secret;
            otp_type.SelectedIndex = otp.Type;
            otp_algorithm.SelectedIndex = otp.Algorithm;
            otp_digits.SelectedItem = otp.Digits;
            otp_period.SelectedItem = otp.Period;
            type = otp.LogoType;
            logo = otp.Logo;
            Refresh_Logo();
        }
        private async void image_change_Click(object sender, RoutedEventArgs e)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["type"] = type;
            localSettings.Values["logo"] = logo;
            ContentDialog chooseLogoDialog = new ChooseLogoDialog
            {
                Title = resourceLoader.GetString("image_choose"),
                PrimaryButtonText = resourceLoader.GetString("ok"),
                SecondaryButtonText = resourceLoader.GetString("cancel2"),
            };

            ContentDialogResult result = await chooseLogoDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                
                if (localSettings.Values["type"] != null)
                {
                    type = (int)localSettings.Values["type"];
                }
                logo = localSettings.Values["logo"] as string;
                Refresh_Logo();
            }
        }
        private void Refresh_Logo()
        {
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
                    otp.Issuer = otp_issuer.Text;
                    otp.Name = otp_label.Text;
                    otp.Secret = otp_secret.Text;
                    otp.Type = otp_type.SelectedIndex;
                    otp.Algorithm = otp_algorithm.SelectedIndex;
                    otp.Digits = (int)otp_digits.SelectedItem;
                    otp.Period = (int)otp_period.SelectedItem;
                    otp.LogoType = type;
                    otp.Logo = logo;
                    await SqlAccess.Update_Item(otp);
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
