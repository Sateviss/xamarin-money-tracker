using System;
using System.Collections.Generic;
using MTracker.Charters;

namespace MTracker.ViewModel
{
    public class ChartViewModel : BaseViewModel
    {
        public List<ICharter> ChartList;

        public ChartViewModel()
        {
            Title = "Charts";
            ChartList = new List<ICharter>
            {
                new CharterMonthlyPie()
            };
        }
    }
}
