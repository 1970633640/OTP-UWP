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
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

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
        public static MainPage mainPage;
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
            mainPage = this;
            init_data();
            Debug.WriteLine("Mainpage Start!");
            //DispatcherTimerSetup();
        }

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            Debug.WriteLine("MainPage Timer start!");
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            GenerateCodes();
            Debug.WriteLine("TICK!");
        }

        public async void init_data()
        {
            otpItems.Clear();
            var x = await SqlAccess.Get_All_Item();
            foreach (var item in x)
            {
                otpItems.Add(new ObserverbleOtp
                {
                    name = item.Name,
                    Code = "000000",
                    issuer = item.Issuer,
                    logo =item.LogoType==1? "ms-appx:///Assets/Logos/" + item.Logo + ".svg":item.Logo+"\0",
                    emojiVisibility=item.LogoType == 2?1.0f:0.0f,
                    iconVIsibility=item.LogoType==1?1.0f:0.0f ,
                    algorithm = item.Algorithm,
                    period = item.Period,
                    digits = item.Digits,
                    type = item.Type,
                    secret = item.Secret,
                    id = item.Id
                });
            }
            GenerateCodes();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DispatcherTimerSetup();
            GenerateCodes();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            dispatcherTimer.Tick -= dispatcherTimer_Tick;
            dispatcherTimer.Stop();
        }
        private void GenerateCodes()
        {
            foreach (var item in otpItems)
            {
                if (item.type == 0)
                {
                    item.Code = TotpCode.GenerateCode(item.secret, item.algorithm, item.digits, item.period);
                    item.Remain = 100 * (item.period - DateTime.UtcNow.Second % item.period) / item.period;
                }
                else if (item.type == 1)
                {
                    item.Code = SteamCode.GenerateCode(item.secret);
                    item.Remain = 100 * (30 - DateTime.UtcNow.Second % 30) / 30;
                }
            }
        }




        private void setting_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Add));
        }

        private void OtpGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            //OtpGrid.Scale=System.Numerics.Vector3(1,1,1) ;
            Debug.WriteLine(((ObserverbleOtp)e.ClickedItem).id.ToString());
            ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            bool copy = true;
            try {copy = (bool)roamingSettings.Values["CLICK_COPY"]; }
            catch { }
            if (copy)
            {
                DataPackage dataPackage = new DataPackage();
                dataPackage.RequestedOperation = DataPackageOperation.Copy;
                dataPackage.SetText(((ObserverbleOtp)e.ClickedItem).Code);
                Clipboard.SetContent(dataPackage);
            }
        }

        private ObserverbleOtp clicked_item;
        private void OtpGrid_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            /* if (e.OriginalSource.GetType() != typeof(Grid))
             {
                 if(e.OriginalSource.GetType()==typeof(GridViewItem))
                 {
                     clicked_item = ((GridViewItem)e.OriginalSource).Content as ObserverbleOtp;
                 }
                 else
                 {
                     clicked_item = (e.OriginalSource as FrameworkElement)?.DataContext as ObserverbleOtp;
                 }
                 Debug.WriteLine(clicked_item.Code);
                 //Debug.WriteLine(((FrameworkElement)e.OriginalSource).DataContext);
                 MenuFlyout myFlyout = new MenuFlyout();
                 var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                 MenuFlyoutItem firstItem = new MenuFlyoutItem { Text = resourceLoader.GetString("copy")};
                 MenuFlyoutItem secondItem = new MenuFlyoutItem { Text = resourceLoader.GetString("edit") };
                 myFlyout.Items.Add(firstItem);
                 myFlyout.Items.Add(secondItem);

                 //if you only want to show in left or buttom 
                 //myFlyout.Placement = FlyoutPlacementMode.Left;

                 FrameworkElement senderElement = sender as FrameworkElement;

                 //the code can show the flyout in your mouse click 
                 myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
             }
         }*/
            var orig = e.OriginalSource as DependencyObject;

            while (orig != null && orig != OtpGrid)
            {
                var lv = orig as GridViewItem;
                if (lv != null)
                {
                    var res1 = (lv.Content as ObserverbleOtp);
                    clicked_item = res1;
                    MenuFlyout.ShowAt(OtpGrid, e.GetPosition(OtpGrid));
                    break;
                }
                orig = VisualTreeHelper.GetParent(orig);
            }
        }

        private void copy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(clicked_item.Code);
            Clipboard.SetContent(dataPackage);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Edit),clicked_item.id);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(About));
        }
    }
}
