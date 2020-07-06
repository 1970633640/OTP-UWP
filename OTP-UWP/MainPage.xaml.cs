using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OtpNet;
using System;
using OTP_UWP.Functions;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace OTP_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var totp = new Totp(Base32Encoding.ToBytes("K5ME2SCOGI2EYQSIJJCTKNKHJ5GECN2UJQ2E6S2HIJFFSTJTINJQ===="), mode: OtpHashMode.Sha1);

            //logos.Text = totp.ComputeTotp();
            
            logos.Text += "   s:" + SteamCode.GenerateCode("SRX5CBQJH2OV2LY4RGAE65XMM7NGACJW");
        }
    }
}
