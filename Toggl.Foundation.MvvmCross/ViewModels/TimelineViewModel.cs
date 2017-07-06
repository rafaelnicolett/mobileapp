using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using Toggl.Foundation.DataSources;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.Foundation.MvvmCross.ViewModels
{
    public class TimelineViewModel : MvxViewModel
    {
        private readonly ITogglDataSource dataSource;

        public ObservableCollection<ITimeEntry> TimeEntries { get; }
            = new ObservableCollection<ITimeEntry>();

        public ObservableCollection<IProject> Projects { get; }
            = new ObservableCollection<IProject>();

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
                            .Select(orderList)
                            .ForEachAsync(TimeEntries.AddRange);
            
            Projects.AddRange(await dataSource.Projects.GetAll());
        }

        private IOrderedEnumerable<ITimeEntry> orderList(IEnumerable<ITimeEntry> timeEntries)
            => timeEntries.OrderBy(te => te.Start);

    }
}
