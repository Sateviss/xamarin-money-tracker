﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MTracker.Data;
using MTracker.Views;
using MTracker.Models;
using MTracker.Resources;
using System.Globalization;
using System.Collections.Generic;
using MTracker.ViewModel;

namespace MTracker
{

    public partial class App : Application
    {
        public static DynamicEntryAccessor EntryAccessor;
        public static SyncedCategoryAccessor CategoryAccessor;

        public static string Currency = "$";

        public App()
        {
            SettingsViewModel.LoadAndApplySettings();
            InitializeComponent();
            EntryAccessor = new DynamicEntryAccessor();
            CategoryAccessor = new SyncedCategoryAccessor();
            EntryAccessor.CollectionChanged += (sender, e) => { System.Diagnostics.Debug.WriteLine("CollectionChanged"); };
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
