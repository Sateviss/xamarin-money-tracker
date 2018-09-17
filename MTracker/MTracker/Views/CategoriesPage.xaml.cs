using System;
using System.Collections.Generic;
using MTracker.ViewModel;

using Xamarin.Forms;

namespace MTracker.Views
{
    public partial class CategoriesPage : ContentPage
    {
        public CategoriesPage()
        {
            InitializeComponent();
            var vm = new CategoriesViewModel();
            BindingContext = vm;
            CategoriesList.ItemsSource = vm.Categories;
            ToolbarItems.Add(new ToolbarItem("", "stack_add_icon.xml", () => {}, priority: 1));
        }
    }
}
