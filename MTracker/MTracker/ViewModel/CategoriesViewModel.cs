using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MTracker.Models;

namespace MTracker.ViewModel
{
    public class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories = App.CategoryAccessor.ObservableList;

        public CategoriesViewModel()
        {
            Title = "Categories";
        }
    }
}
