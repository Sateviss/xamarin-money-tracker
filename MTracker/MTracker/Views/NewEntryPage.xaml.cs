using MTracker.ViewModel;
using Xamarin.Forms;

namespace MTracker.Views
{
    public partial class NewEntryPage : ContentPage
    {
        public NewEntryPage()
        {
            InitializeComponent();
        }

        public NewEntryViewModel viewModel;

        public NewEntryPage(Models.Entry entry)
        {
            viewModel = new NewEntryViewModel(entry);
            BindingContext = viewModel;

            InitializeComponent();

            CategoryPicker.ItemsSource = viewModel.Categories;

            CancelButton.Clicked += (sender, e) => viewModel.Cancel();
            AcceptButton.Clicked += (sender, e) => viewModel.Accept();
            TitleEntry.IsTextPredictionEnabled = true;

            viewModel.OnTitleError += () => { ErrorTitle.FadeTo(1); };
            viewModel.OnCategoryError += () => { ErrorPicker.FadeTo(1); };
            viewModel.OnAmountError += () => { ErrorValue.FadeTo(1); };

            CategoryPicker.SelectedIndexChanged += (sender, e) => { ErrorPicker.FadeTo(0); };
            TitleEntry.TextChanged += (sender, e) => { ErrorTitle.FadeTo(0); };
            AmountEntry.TextChanged += (sender, e) => { ErrorValue.FadeTo(0); };
        }
    }
}
