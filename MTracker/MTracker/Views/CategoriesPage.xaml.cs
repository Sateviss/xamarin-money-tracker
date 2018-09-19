using MTracker.Models;
using MTracker.ViewModel;
using Xamarin.Forms;

namespace MTracker.Views
{
    public partial class CategoriesPage : ContentPage
    {
        public CategoriesPage()
        {
            InitializeComponent();
            var vm = new CategoriesViewModel(this);
            BindingContext = vm;
            CategoriesList.ItemsSource = vm.Categories;
            ToolbarItems.Add(new ToolbarItem("",
                                             "stack_add_icon.xml", async () => { await vm.AddCategory(); },
                                             priority: 1));
            CategoriesList.ItemSelected += async (sender, e) =>
            {

                if (e.SelectedItem == null)
                    return;
                ((ListView)sender).SelectedItem = null;
                await vm.EditCategory((Category)e.SelectedItem);                       
            };
        }
    }
}
