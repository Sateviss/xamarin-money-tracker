using System;
using System.Collections.Generic;
using System.Linq;
using Microcharts;
using SkiaSharp;

namespace MTracker.Charters
{
	public class CharterDailyBars : ICharter
    {
        private List<Models.Entry> data;
        private List<Entry> filteredData;
        private readonly PointChart chart;

        public CharterDailyBars()
        {
            data = App.EntryAccessor.GetFromDate(DateTime.Now - TimeSpan.FromDays(7));
            filteredData = new List<Entry>();

            for (int i = 7; i >= 0; i--)
            {
                var day = (DateTime.Now - TimeSpan.FromDays(i)).Date;
                var sum = data.Where((obj) => obj.Date.Date == day).Sum((arg) => arg.Amount);
                var entry = new Entry(sum)
                {
                    Color = SKColor.Parse("#FFFFFF"),
                    Label = day.DayOfWeek.ToString(),
                    ValueLabel = sum.ToString("F2")
                };
                filteredData.Add(entry);
            }

            chart = new BarChart
            {
                BackgroundColor = SKColor.Parse("#00000000"),
                Entries = filteredData,
                LabelTextSize = 40
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
            return "Daily spending (last 7 days)";
        }
    }
}
