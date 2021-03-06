﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microcharts;
using MTracker.Resources;
using SkiaSharp;

namespace MTracker.Charters
{
    public class CharterMonthlyPie : ICharter
    {
        private List<Models.Entry> data;
        private List<Entry> filteredData;
        private readonly DonutChart chart;

        public CharterMonthlyPie()
        {
            data = App.EntryAccessor.GetFromDate(DateTime.Now-TimeSpan.FromDays(30));
            filteredData = new List<Entry>();
            foreach (var category in App.CategoryAccessor.ObservableList)
            {
                var val = data.Where((arg) => { return arg.CategoryID == category.ID - 1; }).Sum((arg) => { return arg.Amount; });
                filteredData.Add(new Entry(val) { Label = category.Name, Color = SKColor.Parse(category.Color), ValueLabel = val.ToString("F2") });
            }
            chart = new DonutChart
            {
                BackgroundColor = SKColor.Parse("#00000000"),
                Entries = filteredData,
                LabelTextSize = 40,
                HoleRadius = 0.6f
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
            return Text.MonthlyPieLabel;
        }
    }
}
