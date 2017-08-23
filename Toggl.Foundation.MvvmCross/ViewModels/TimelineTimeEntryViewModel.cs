using System;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.MvvmCross.ViewModels
{
    public class TimelineTimeEntryViewModel
    {
        private const string defaultColor = "#CECECE";

        public DateTimeOffset Start { get; }

        public DateTimeOffset Stop { get; }

        public string ProjectColor { get; }

        public TimelineTimeEntryViewModel(IDatabaseTimeEntry timeEntry)
        {
            Ensure.Argument.IsNotNull(timeEntry, nameof(timeEntry));

            if (timeEntry.Stop == null)
                throw new ArgumentException($"Can't use running time entry ({nameof(Stop)} must be set).");

            Start = timeEntry.Start;
            Stop = timeEntry.Stop.Value;
            ProjectColor = timeEntry?.Project?.Color ?? defaultColor;
        }
    }
}
