using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using SkiaSharp;
using MTracker.Charters;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : ContentPage
    {
        public ChartPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var chart = new CharterMonthlyPie();
            meChartView.Chart = chart.GetChart();
            meChartView.HeightRequest = chart.GetHeight();
            Label.Text = chart.GetLabel();
        }
    }
}
