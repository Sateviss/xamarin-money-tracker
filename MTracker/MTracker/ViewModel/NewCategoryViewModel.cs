using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using MTracker.Models;
using MTracker.Views;
using Xamarin.Forms;

namespace MTracker.ViewModel
{
    public class NewCategoryViewModel : BaseViewModel
    {
        public Category Category;

        public TaskCompletionSource<Category> taskCompletion;

        public string Color
        {
            get => Category.Color ?? "#202020";
            set
            {
                Category.Color = value;
                OnPropertyChanged("Color");
                OnPropertyChanged("InvertedColor");
                OnPropertyChanged("ColorR");
                OnPropertyChanged("ColorG");
                OnPropertyChanged("ColorB");
            }
        }
        public Color InvertedColor 
        {
            get
            {
                var l = Xamarin.Forms.Color.FromHex(Color).Luminosity;
                return Xamarin.Forms.Color.FromHsla( 0, 0, l < 0.5 ? (1 - l * l) : ((1 - l) * (1 -l)));
            }
        }

        public int ColorR
        {
            get => int.Parse(Color.Substring(1, 2), NumberStyles.HexNumber);
            set => Color = Color.ReplaceSubstring(1, 2, value.ToString("X2"));
        }

        public int ColorG
        {
            get => int.Parse(Color.Substring(3, 2), NumberStyles.HexNumber);
            set => Color = Color.ReplaceSubstring(3, 2, value.ToString("X2"));
        }

        public int ColorB
        {
            get => int.Parse(Color.Substring(5, 2), NumberStyles.HexNumber);
            set => Color = Color.ReplaceSubstring(5, 2, value.ToString("X2"));
        }

        public string Name
        {
            get => Category.Name;
            set
            {
                Category.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public event Action OnNameError;

        public void Accept()
        {
            if (Category.Name == null)
            {
                OnNameError();
            }
            else
            {
                taskCompletion.SetResult(Category);
            }
        }

        public void Cancel()
        {
            taskCompletion.SetResult(null);
        }


        public NewCategoryViewModel(Category myCategory)
        {
            Title = myCategory.ID == 0 ? "New category" : "Editing category";
            Category = myCategory;
            taskCompletion = new TaskCompletionSource<Category>();
        }

        public static async Task<Category> EditCategory(Page currentPage, Category category)
        {
            var page = new NewCategoryPage(category);
            await currentPage.Navigation.PushModalAsync(new NavigationPage(page));
            await ((NewCategoryViewModel)page.BindingContext).taskCompletion.Task;
            Category result = ((NewCategoryViewModel)page.BindingContext).taskCompletion.Task.Result;
            await currentPage.Navigation.PopModalAsync();
            return result;
        }
    }

    public static class StringReplaceSubstring
    {
        public static string ReplaceSubstring(this string me, int startingIndex, int length, string replacement)
        {
            me = me.Substring(0, startingIndex) + replacement + me.Substring(startingIndex + length, me.Length - startingIndex - length);
            return me;
        }
    }
}
