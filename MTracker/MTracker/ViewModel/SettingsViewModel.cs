using System;
using System.Collections.Generic;
using MTracker.Models;
using MTracker.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;

namespace MTracker.ViewModel
{
	public class SettingsViewModel : BaseViewModel
    {
        public List<Language> Languages = new List<Language>{
            new Language{ Locale = "en-US", Name = Text.EnglishLabel},
            new Language{ Locale = "ru-RU", Name = Text.RussianLabel}
        };

        private int selectedIndex;
        public int SelectedIndex 
        {
            get => selectedIndex;
            set 
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        private string currency;
        public string Currency 
        {
            get => currency;
            set
            {
                currency = value.Substring(0, Math.Min(3, value.Length));
                OnPropertyChanged("Currency");
            }
        }

        public void SaveToJSON()
        {
            var obj = new JObject
            {
                { "currency", Currency },
                { "locale", Languages[SelectedIndex].Locale}
            };
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "settings.json"));
            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "settings.json"), obj.ToString());
        }

        public void LoadFromJSON()
        {
            var exists = File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "settings.json"));
            if (exists)
            {
                try
                {
                    var data = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "settings.json"));
                    var obj = JObject.Parse(data);
                    Currency = obj.GetValue("currency").ToString();
                    SelectedIndex = Languages.FindIndex((x) => { return x.Locale == obj.GetValue("locale").ToString(); });
                    return;
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("err");
                }
            }
            Currency = "$";
            SelectedIndex = 0;
        }

        public void ApplySettings()
        {
            Text.Culture = new System.Globalization.CultureInfo(Languages[SelectedIndex].Locale);
            App.Currency = Currency;
        }

        public void Apply()
        {
            SaveToJSON();
            ApplySettings();
        }

        public void Discard()
        {
            LoadFromJSON();
        }

        public static void LoadAndApplySettings()
        {
            var loader = new SettingsViewModel();
            loader.LoadFromJSON();
            loader.ApplySettings();
        }

        public SettingsViewModel()
        {
            LoadFromJSON();
            Title = Text.SettingsPageLabel;
        }
    }
}
