using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using OtpNet;
using OTP_UWP.Functions;
using System.ComponentModel;
using OTP_UWP.DataClass;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace OTP_UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<ObserverbleOtp> otpItems = new ObservableCollection<ObserverbleOtp>();
        private DispatcherTimer dispatcherTimer;
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            init_data();
            DispatcherTimerSetup();
        }

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            GenerateCodes();
        }

        private async void init_data()
        {
            var x=await SqlAccess.Get_All_Item();
            foreach (var item in x)
            {
                otpItems.Add(new ObserverbleOtp
                {
                    name = item.Name,
                    Code = "000000",
                    issuer = item.Issuer,
                    logo = "ms-appx:///Assets/Logos/" + item.Logo + ".svg",
                    algorithm = item.Algorithm,
                    period = item.Period,
                    digits = item.Digits,
                    type = item.Type,
                    secret = item.Secret
                }) ; 
            }
            GenerateCodes();
        }

        private void GenerateCodes()
        {
            foreach (var item in otpItems)
            {
                if (item.type == 0)
                {
                    item.Code = TotpCode.GenerateCode(item.secret, item.algorithm, item.digits, item.period);
                    item.Remain = 100 * (item.period-DateTime.UtcNow.Second % item.period)/item.period;
                }
                else if (item.type == 1)
                {
                    item.Code = SteamCode.GenerateCode(item.secret);
                    item.Remain = 100*(30-DateTime.UtcNow.Second % 30)/30;
                }
            }
        }
        



        private void setting_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Add));
        }
    }
}
