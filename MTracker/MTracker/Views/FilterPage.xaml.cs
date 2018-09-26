using System;
using System.Collections.Generic;
using MTracker.ViewModel;
using Xamarin.Forms;

namespace MTracker.Views
{
    public partial class FilterPage : ContentPage
    {
        private FilterViewModel vm;

        public FilterPage()
        {
            InitializeComponent();
            vm = new FilterViewModel();
            BindingContext = vm;
        }
    }
}
