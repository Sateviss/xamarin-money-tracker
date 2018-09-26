using System;
using System.Collections.Generic;
using MTracker.Models;
using MTracker.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class MenuPage : ContentPage
    {
        MainPage MainPage { get => (MainPage)Application.Current.MainPage; }

        class MenuItem
        {
            public int ID { get; set; }
            public String Title { get; set; }
        }

        List<MenuItem> Items;

        public MenuPage()
        {
            InitializeComponent();

            Items = new List<MenuItem>{
                new MenuItem {ID = (int)MenuItemType.Charts, Title = Text.ChartPageLabel},
                new MenuItem {ID = (int)MenuItemType.Entries, Title = Text.EntriesPageLabel},
                new MenuItem {ID = (int)MenuItemType.Categories, Title = Text.CategoriesPageLabel},
                new MenuItem {ID = (int)MenuItemType.About, Title = Text.AboutPageLabel}
            };

            ListViewMenu.ItemsSource = Items;
            ListViewMenu.SelectedItem = Items[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                await MainPage.NavigateToPage(((MenuItem)((ListView)sender).SelectedItem).ID);
            };

        }
    }
}
