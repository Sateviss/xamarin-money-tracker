using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MTracker.Models;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public static Dictionary<int, NavigationPage> Pages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            Master = new MenuPage();

            InitializeComponent();
        }

        public async Task NavigateToPage(int pageId)
        {
            if (!Pages.ContainsKey(pageId))
            {
                switch ((MenuItemType)pageId)
                {
                    case MenuItemType.About:
                        Pages.Add(pageId, new NavigationPage(new AboutPage()));
                        break;
                    case MenuItemType.Charts:
                        Pages.Add(pageId, new NavigationPage(new ChartPage()));
                        break;
                    case MenuItemType.Entries:
                        Pages.Add(pageId, new NavigationPage(new EntriesPage()));
                        break;
                }
            }

            var newPage = Pages[pageId];
            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;
                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

            }
        }
    }
}
