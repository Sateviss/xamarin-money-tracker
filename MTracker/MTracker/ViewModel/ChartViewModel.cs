using System.Collections.Generic;
using MTracker.Charters;

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
            Title = "Charts";
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
