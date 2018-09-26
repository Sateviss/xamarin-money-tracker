using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MTracker.Models;
using MTracker.Resources;
using Xamarin.Forms;

namespace MTracker.ViewModel
{
    public class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories = App.CategoryAccessor.ObservableList;
        public ContentPage page;

        public CategoriesViewModel(ContentPage Page)
        {
            page = Page;
            Title = Text.CategoriesPageLabel;
        }

        public async Task AddCategory()
        {
            var res = await NewCategoryViewModel.EditCategory(page, new Models.Category());
            if (res == null)
                return;
            Categories.Add(res);
            OnPropertyChanged("Categories");
        }

        public async Task EditCategory(Category category)
        {
            var i = Categories.IndexOf(category);
            var res = await NewCategoryViewModel.EditCategory(page, category);
            if (res != null)
                Categories[i] = res;
            OnPropertyChanged("Categories");
        }
    }
}
