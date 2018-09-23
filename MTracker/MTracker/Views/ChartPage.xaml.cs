using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using SkiaSharp;
using MTracker.Charters;
using MTracker.ViewModel;
using System.Collections.Generic;
using Microcharts.Forms;
using System.Linq;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : ContentPage
    {
        public ChartViewModel vm;
        public List<StackLayout> stacks = new List<StackLayout>();

        public ChartPage()
        {
            InitializeComponent();
            vm = new ChartViewModel();
            BindingContext = vm;
            vm.Update = true;
            PullToRefresh.RefreshCommand = new Command(() => {
                vm.ReloadData();
                ReloadCharts();
                PullToRefresh.IsRefreshing = false;
            });

        }

        private void ReloadCharts()
        {
            var list = new List<View>();
            foreach (var chart in vm.ChartList)
            {
                var CV = new ChartView
                {
                    Chart = chart.GetChart(),
                    HeightRequest = chart.GetHeight(),
                    BindingContext = chart
                };

                var L = new Label
                {
                    Text = chart.GetLabel(),
                    FontSize = 16,
                    HorizontalTextAlignment = TextAlignment.Center,
                    IsEnabled = false
                };

                var SL = new StackLayout();
                SL.Children.Add(CV);
                SL.Children.Add(L);

                var F = new Frame
                {
                    Content = SL,
                    Padding = 8,
                    Margin = 0,
                    CornerRadius = 0,
                    BackgroundColor = Color.Transparent,
                    BorderColor = Color.Black
                };

                list.Add(F);
            }
            Scroll.Children.Clear();
            list.ForEach((obj) => Scroll.Children.Add(obj));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!vm.Update)
                return;
            ReloadCharts();
            vm.Update = false;
        }
    }
}
