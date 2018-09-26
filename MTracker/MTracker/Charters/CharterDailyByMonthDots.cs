using System;
using System.Collections.Generic;
using System.Linq;
using Microcharts;
using MTracker.Resources;
using SkiaSharp;

namespace MTracker.Charters
{
    public class CharterDailyByMonthDots : ICharter
    {
        private List<Models.Entry> data;
        private List<Entry> filteredData;
        private readonly PointChart chart;

        public CharterDailyByMonthDots()
        {
            data = App.EntryAccessor.GetFromDate(DateTime.Now - TimeSpan.FromDays(365));
            filteredData = new List<Entry>();

            for (int i = 11; i >= 0; i--)
            {
                var month = DateTime.Now.AddMonths(-i);
                var thisMonthData = data.Where((arg) => arg.Date.Month == month.Month && arg.Date.Year == month.Year);
                var daysInMonth = i == 0? DateTime.Now.Day:(!thisMonthData.Any() ? 30 : DateTime.DaysInMonth(thisMonthData.First().Date.Year, thisMonthData.First().Date.Month));
                var sum = thisMonthData.Sum((arg) => arg.Amount) / daysInMonth;
                var entry = new Entry(sum)
                {
                    Color = SKColor.Parse("#FFFFFF"),
                    Label = month.ToString("MMM", Text.Culture),
                    ValueLabel = sum.ToString("F2")
                };
                filteredData.Add(entry);
            }

            chart = new PointChart
            {
                BackgroundColor = SKColor.Parse("#00000000"),
                Entries = filteredData,
                LabelTextSize = 40,
                PointMode = PointMode.Circle,
                PointAreaAlpha = 128
            };
        }

        public Chart GetChart()
        {
            return chart;
        }

        public int GetHeight()
        {
            return 200;
        }

        public string GetLabel()
        {
            return Text.DalyByMonthLabel;
        }
    }
}
