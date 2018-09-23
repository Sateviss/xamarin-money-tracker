using System;

using Xamarin.Forms;


namespace MTracker.ViewModel
{
    public class AboutViewModel : BaseViewModel
    {
        public const string BerezhURL = "https://www.bsuir.by/ru/kaf-informatiki/berezhnov-d-e";
        public const string FKSISURL = "http://fksis.bsuir.by/";
        public const string GitHubURL = "https://github.com/Sateviss/xamarin-money-tracker";
        public const string XamarinURL = "https://docs.microsoft.com/en-us/xamarin/xamarin-forms/";

        public AboutViewModel()
        {
            Title = "About";
        }

        public void OpenBerezh() => Device.OpenUri(new Uri(BerezhURL));
        public void OpenFKSIS() => Device.OpenUri(new Uri(FKSISURL));
        public void OpenGitHub() => Device.OpenUri(new Uri(GitHubURL));
        public void OpenXamarin() => Device.OpenUri(new Uri(XamarinURL));
    }
}