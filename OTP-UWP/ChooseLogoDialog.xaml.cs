using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace OTP_UWP
{

    public sealed partial class ChooseLogoDialog : ContentDialog
    {
        public static ChooseLogoDialog chooseLogo;
        private int type = 0;
        private string logo = "";

        public ChooseLogoDialog()
        {
            this.InitializeComponent();
            choose_svg.Navigate(typeof(ChooseSvg));
            chooseLogo = this;

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["type"] != null)
            {
                type =(int) localSettings.Values["type"] ;
            }
            logo = localSettings.Values["logo"] as string;
            current.Text = "type:" + type + " value:" + logo;
        }

        public void RefreshStatus(int t,string l)
        {
            type = t;
            logo = l;
            current.Text = "type:" + t + " value:" + l;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["type"] =type;
            localSettings.Values["logo"] = logo;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void choose_none_Click(object sender, RoutedEventArgs e)
        {
            RefreshStatus(0, "");
        }
    }
}
