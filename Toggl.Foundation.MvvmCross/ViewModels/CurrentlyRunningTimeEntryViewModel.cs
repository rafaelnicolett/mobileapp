using System;
using MvvmCross.Core.ViewModels;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.MvvmCross.ViewModels
{
    [Preserve(AllMembers = true)]
    public sealed class CurrentlyRunningTimeEntryViewModel : MvxNotifyPropertyChanged
    {
        public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;

        public long Id { get; }

        public string Description { get; }

        public DateTimeOffset Start { get; }

        public CurrentlyRunningTimeEntryViewModel(IDatabaseTimeEntry timeEntry, ITimeService timeService)
        {
            Ensure.Argument.IsNotNull(timeEntry, nameof(timeEntry));
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));

            Id = timeEntry.Id;
            Start = timeEntry.Start;
            Description = timeEntry.Description;

            timeService
                .CurrentDateTimeObservable
                .Subscribe(currentTime => ElapsedTime = currentTime - Start);
        }
    }
}
