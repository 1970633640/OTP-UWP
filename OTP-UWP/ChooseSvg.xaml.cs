using OTP_UWP.DataClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public sealed partial class ChooseSvg : Page
    {
        private ObservableCollection<SvgItem> svgItems = new ObservableCollection<SvgItem>();
        private List<SvgItem> allSvgItems =new List<SvgItem>();
        public ChooseSvg()
        {
            this.InitializeComponent();
            initSvg();
        }
        public async void initSvg()
        {
            var x = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var xx = await x.GetFolderAsync("Logos");
            var svgs = await xx.GetFilesAsync();
            foreach (var svg in svgs)
            {
                allSvgItems.Add(new SvgItem {name=svg.DisplayName,path= "ms-appx:///Assets/Logos/"+svg.Name });
                searchSvg("");
            }
        }
        public void searchSvg(string key)
        {
            svgItems.Clear();
            if (key == " ")
            {
                foreach (var item in allSvgItems)
                {
                    svgItems.Add(item);
                }
            }
            else
            {
                foreach (var item in allSvgItems)
                {
                    if(item.name.IndexOf(key,StringComparison.OrdinalIgnoreCase)!=-1)
                    svgItems.Add(item);
                }
            }
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchSvg(search.Text.Trim());
        }

        private void svgGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            ChooseLogoDialog.chooseLogo.RefreshStatus(1, ((SvgItem)e.ClickedItem).name);
        }
    }
}
