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
            foreach (var chart in vm.ChartList)
            {
                var SL = new StackLayout();
                var CV = new ChartView();
                CV.BindingContext = chart;
                var L = new Label();
                SL.Children.Add(CV);
                SL.Children.Add(L);
                stacks.Add(SL);

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            foreach (var SL in stacks)
            {
                var CV = SL.Children.First((arg) => arg is ChartView) as ChartView;
                var charter = CV.BindingContext as ICharter;
                CV.Chart = charter.GetChart();
                CV.HeightRequest = charter.GetHeight();

                var L = SL.Children.First((arg) => arg is Label) as Label;
                L.Text = charter.GetLabel();
                L.FontSize = 20;
                L.HorizontalTextAlignment = TextAlignment.Center;
                L.IsEnabled = false;
                Scroll.Children.Add(SL);
            }
        }
    }
}
