using System;
using System.Collections.Generic;
using MTracker.Models;
using Xamarin.Forms;

namespace MTracker.Views
{
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
                new MenuItem {ID = (int)MenuItemType.Charts, Title = "Charts"},
                new MenuItem {ID = (int)MenuItemType.Entries, Title = "Entries"},
                new MenuItem {ID = (int)MenuItemType.Categories, Title = "Categories"},
                new MenuItem {ID = (int)MenuItemType.About, Title = "About"}
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
