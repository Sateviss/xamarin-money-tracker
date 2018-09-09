using System;
using System.Collections.Generic;
using MTracker.Models;
using MTracker.Data;

namespace MTracker.ViewModel
{
    public class EntriesViewModel : BaseViewModel
    {

        public IEnumerable<Entry> Entries { get; private set; }

        public static DynamicEntryAccessor entryAccessor;

        public EntriesViewModel()
        {
            Title = "Entries";

            entryAccessor = new DynamicEntryAccessor();

            Entries = entryAccessor;
        }
    }
}
