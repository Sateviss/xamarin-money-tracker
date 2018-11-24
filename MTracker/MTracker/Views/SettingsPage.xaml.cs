using System;
using System.Collections.Generic;
using MTracker.Resources;
using MTracker.ViewModel;
using Xamarin.Forms;

namespace MTracker.Views
{
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel viewModel = new SettingsViewModel();

        public SettingsPage()
        {
            BindingContext = viewModel;
            InitializeComponent();
            LangPicker.ItemsSource = viewModel.Languages;
            LanguageLabel.Text = Text.LanguageLabel;
            CurrencyLabel.Text = Text.CurrencyLabel;
            DiscardButton.Text = Text.DiscardLabel;
            ApplyButton.Text = Text.ApplyLabel;
            viewModel.Discard();
            DiscardButton.Clicked += (sender, e) => { viewModel.Discard(); };
            ApplyButton.Clicked += (sender, e) => { viewModel.Apply(); DisplayAlert("", Text.Restart, Text.Ok); };
        }
    }
}
