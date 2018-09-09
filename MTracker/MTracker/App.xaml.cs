using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MTracker.Data;
using MTracker.Views;

namespace MTracker
{
    public partial class App : Application
    {
        static DynamicEntryAccessor accessor;

        public static DynamicEntryAccessor EntryAccessor 
        {
            get 
            {
                if (accessor == null)
                    accessor = new DynamicEntryAccessor();
                return accessor;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
