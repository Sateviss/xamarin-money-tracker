using MTracker.Models;
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

            CancelButton.Clicked += (sender, e) => viewModel.Cancel();
            AcceptButton.Clicked += (sender, e) => viewModel.Accept();
            DeleteButton.Clicked += (sender, e) => viewModel.Delete();

            viewModel.OnNameError += () => { ErrorName.FadeTo(1); };
            NameEntry.TextChanged += (sender, e) => { ErrorName.FadeTo(0); };
        }
    }
}
