using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using Toggl.Foundation.DataSources;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.MvvmCross.ViewModels
{
    public sealed class TimelineViewModel : MvxViewModel
    {
        private readonly ITogglDataSource dataSource;

        public ObservableCollection<TimeLineTimeEntryViewModel> TimeEntries { get; }
            = new ObservableCollection<TimeLineTimeEntryViewModel>();

        public TimelineViewModel(ITogglDataSource dataSource)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));

            this.dataSource = dataSource;
        }

        public async override Task Initialize()
        {
            await base.Initialize();

            await dataSource.TimeEntries
                            .GetAll()
                            .Select(fromThisWeek)
                            .Select(notCurrentlyRunning)
                            .Select(orderList)
                            .Select(timeEntries => timeEntries.Select(te => new TimeLineTimeEntryViewModel(te)))
                            .Do(TimeEntries.AddRange);
        }
        
        private IEnumerable<IDatabaseTimeEntry> fromThisWeek(IEnumerable<IDatabaseTimeEntry> timeEntries)
        {
            var week = TimeSpan.FromDays(7);
            return timeEntries.Where(te =>
            {
                var delta = DateTime.Now - te.Start;
                return delta <= week;
            });
        }

        private IEnumerable<IDatabaseTimeEntry> notCurrentlyRunning(IEnumerable<IDatabaseTimeEntry> timeEntries)
            => timeEntries.Where(te => te.Stop != null);

        private IOrderedEnumerable<IDatabaseTimeEntry> orderList(IEnumerable<IDatabaseTimeEntry> timeEntries)
            => timeEntries.OrderBy(te => te.Start);
    }
}
