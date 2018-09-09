using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MTracker.ViewModel;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntriesPage : ContentPage
    {
        EntriesViewModel EntriesViewModel;

        public EntriesPage()
        {
            InitializeComponent();

            EntriesViewModel = new EntriesViewModel();

            EntriesList.ItemsSource = EntriesViewModel.Entries;
        }
    }
}
