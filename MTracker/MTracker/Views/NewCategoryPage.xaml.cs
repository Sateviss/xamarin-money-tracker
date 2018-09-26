using MTracker.Models;
using MTracker.Resources;
using MTracker.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCategoryPage : ContentPage
    {
        public NewCategoryPage(Category category)
        {

            var viewModel = new NewCategoryViewModel(category);
            BindingContext = viewModel;
            InitializeComponent();

            ColorLabel.Text = Text.ColorLabel;
            RedLabel.Text = Text.RedLabel;
            GreenLabel.Text = Text.GreenLabel;
            BlueLabel.Text = Text.BlueLabel;

            NameEntry.Placeholder = Text.NameLabel;

            CancelButton.Text = Text.CancelLabel;
            AcceptButton.Text = Text.AcceptLabel;

            CancelButton.Clicked += (sender, e) => viewModel.Cancel();
            AcceptButton.Clicked += (sender, e) => viewModel.Accept();
            DeleteButton.Clicked += (sender, e) => viewModel.Delete();

            viewModel.OnNameError += () => { ErrorName.FadeTo(1); };
            NameEntry.TextChanged += (sender, e) => { ErrorName.FadeTo(0); };
        }
    }
}
