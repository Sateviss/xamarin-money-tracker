using MTracker.Resources;
using MTracker.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTracker.Views
{

    [XamlCompilation(XamlCompilationOptions.Skip)]
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

            CategoryPicker.Title = Text.CategoryPickerLabel;
            TitleEntry.Placeholder = Text.EntryTitleEntryLabel;
            AmountEntry.Placeholder = Text.AmountEditorLabel;

            CancelButton.Text = Text.CancelLabel;
            AcceptButton.Text = Text.AcceptLabel;

            CategoryPicker.ItemsSource = viewModel.Categories;
            System.Diagnostics.Debug.WriteLine(viewModel.Categories.IndexOf(viewModel.SelectedCategory));
            CategoryPicker.SelectedIndex = viewModel.Categories.IndexOf(viewModel.SelectedCategory);

            CancelButton.Clicked += (sender, e) => viewModel.Cancel();
            AcceptButton.Clicked += (sender, e) => viewModel.Accept();
            CurrencyLabel.Text = App.Currency;
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
