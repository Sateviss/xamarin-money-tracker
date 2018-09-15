using System;
using System.Globalization;
using System.Threading.Tasks;
using MTracker.Models;
using MTracker.Views;
using Xamarin.Forms;

namespace MTracker.ViewModel
{
    public class NewEntryViewModel : BaseViewModel
    {
        public Models.Entry Entry;

        public TaskCompletionSource<Models.Entry> taskCompletion;

        private TimeSpan time;
        public TimeSpan Time 
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                Entry.Date = Entry.Date + time;
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                Entry.Date = date + Entry.Date.TimeOfDay;
            }
        }

        public string EntryTitle
        {
            get => Entry.Name;
            set => Entry.Name = value;
        }

        public int CategoryID
        {
            get => Entry.CategoryID;
            set => Entry.CategoryID = value;
        }

        public string Amount
        {
            get => Math.Abs(Entry.Amount) < 0.001f ? null : Entry.Amount.ToString();
            set => Entry.Amount = float.Parse(value);
        }

        //public event Action OnCategoryError;
        public event Action OnTitleError;
        public event Action OnAmountError;

        public void Accept()
        {
            bool allClear = true;
            //if (CategoryID == -1)
            //{
            //    allClear = false;
            //    OnCategoryError();
            //}
            if (EntryTitle == "" || EntryTitle == null)
            {
                allClear = false;
                OnTitleError();
            }
            if (Amount == null)
            {
                allClear = false;
                OnAmountError();
            }
            if (allClear)
                taskCompletion.SetResult(Entry);
        }

        public void Cancel()
        {
            taskCompletion.SetResult(null);
        }


        public NewEntryViewModel(Models.Entry myEntry)
        {
            Title = "New entry";
            Entry = myEntry;
            date = Entry.Date;
            time = Entry.Date.TimeOfDay;
            taskCompletion = new TaskCompletionSource<Models.Entry>();
        }

        public static async Task<Models.Entry> EditEntry(Page currentPage, Models.Entry entry)
        {
            var page = new NewEntryPage(entry);
            await currentPage.Navigation.PushModalAsync(new NavigationPage(page));
            await ((NewEntryViewModel)page.BindingContext).taskCompletion.Task;
            Models.Entry result = ((NewEntryViewModel)page.BindingContext).taskCompletion.Task.Result;
            await currentPage.Navigation.PopModalAsync();
            return result;
        }
    }
}
