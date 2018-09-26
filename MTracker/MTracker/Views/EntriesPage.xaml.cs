using System;
using MTracker.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntriesPage : ContentPage
    {
        EntriesViewModel vm;
        private Action addAction;

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
            EntriesList.ItemAppearing += (sender, e) => {
                vm.LoadMore((Models.Entry)e.Item); 
            };
            addAction = vm.ClearSelection;
            MainPage.OnAdd += addAction;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            vm.ClearSelection();
            MainPage.OnAdd -= addAction;
        }
    }
}
