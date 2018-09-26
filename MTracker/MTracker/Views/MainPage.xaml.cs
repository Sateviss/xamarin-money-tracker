using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MTracker.Models;
using MTracker.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public static Dictionary<int, NavigationPage> Pages = new Dictionary<int, NavigationPage>();

        private ToolbarItem Add;

        public MainPage()
        {
            Master = new MenuPage();
            Detail = new NavigationPage(new ChartPage());
            InitializeComponent();

            Add = new ToolbarItem("", "add_icon.xml", async () => { await AddItem(); });
            ToolbarItems.Add(Add);
            OnAdd += () => { };
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
                    case MenuItemType.Categories:
                        Pages.Add(pageId, new NavigationPage(new CategoriesPage()));
                        break;
                }
            }

            var newPage = Pages[pageId];
            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;
                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);
                IsPresented = false;
            }
        }

        public static event Action OnAdd;

        private async Task AddItem()
        {
            var newItem = await NewEntryViewModel.EditEntry(this.Detail, new Models.Entry());
            if (newItem != null)
            {
                OnAdd();
                await App.EntryAccessor.AddAsync(newItem);
            }
        }
    }
}
