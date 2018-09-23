using System;
using Microcharts;

namespace MTracker.Charters
{
    public interface ICharter
    {
        Chart GetChart();
        String GetLabel();
        int GetHeight();
    }
}
