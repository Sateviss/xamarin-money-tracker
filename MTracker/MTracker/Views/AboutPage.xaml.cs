using MTracker.Resources;
using MTracker.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            var vm = new AboutViewModel();
            BindingContext = vm;

            GitHubLabel.Text = Text.GitHubLabel;
            XamarinLabel.Text = Text.XamarinLabel;
            FKSiSLabel.Text = Text.FKSiSLabel;
            BerezhLabel.Text = Text.BerezhLabel;

            InfoLabel.Text = Text.InfoLabel;
            LicenseLabel.Text = Text.LicenseLabel;
            CopyrightLabel.Text = Text.CopyrightLabel;

            BerezhFrame.GestureRecognizers.Add(new TapGestureRecognizer((obj) => vm.OpenBerezh()));
            FKSISFrame.GestureRecognizers.Add(new TapGestureRecognizer((obj) => vm.OpenFKSIS()));
            GitHubFrame.GestureRecognizers.Add(new TapGestureRecognizer((obj) => vm.OpenGitHub()));
            XamarinFrame.GestureRecognizers.Add(new TapGestureRecognizer((obj) => vm.OpenXamarin()));
        }
    }
}
