﻿using System.Collections.Generic;
using MTracker.Charters;
using MTracker.Resources;

namespace MTracker.ViewModel
{
    public class ChartViewModel : BaseViewModel
    {
        public List<ICharter> ChartList;
        private bool _update;
        public bool Update { 
            get => _update; 
            set
            {
                _update = value;
                OnPropertyChanged("Update");
            }
        }

        public ChartViewModel()
        {
            Title = Text.ChartPageLabel;
            ReloadData();
        }

        public void ReloadData()
        {
            ChartList = new List<ICharter>
            {
                new CharterMonthlyPie(),
                new CharterDailyBars(),
                new CharterDailyByMonthDots()
            };
        }
    }
}
