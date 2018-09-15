using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MTracker.ViewModel;
using MTracker.Data;
using System.Threading;
using System.Threading.Tasks;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntriesPage : ContentPage
    {
        EntriesViewModel vm;

        public EntriesPage()
        {
            InitializeComponent();

            vm = new EntriesViewModel(this);
            BindingContext = vm;
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "ToolbarItems")
                {
                    ToolbarItems.Clear();
                    vm.ToolbarItems.ForEach((obj) => { ToolbarItems.Add(obj); });
                }
            };
            EntriesList.ItemAppearing += (sender, e) => { vm.LoadMore((Models.Entry)e.Item); };
        }
    }
}
