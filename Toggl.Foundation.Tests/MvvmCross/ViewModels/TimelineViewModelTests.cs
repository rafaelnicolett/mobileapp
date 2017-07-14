using System;
using System.Reactive.Linq;
using FluentAssertions;
using FsCheck.Xunit;
using NSubstitute;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Multivac.Models;
using Toggl.Ultrawave;
using Toggl.Ultrawave.Models;
using Xunit;
using static Toggl.Multivac.Extensions.FunctionalExtensions;

namespace Toggl.Foundation.Tests.MvvmCross.ViewModels
{
    public class TimelineViewModelTests
    {
        public class TheConstructor
        {
            [Fact]
            public void ThrowsIfTheArgumentsIsNull()
            {
                Action tryingToConstructWithEmptyParameters =
                    () => new TimelineViewModel(null);

                tryingToConstructWithEmptyParameters
                    .ShouldThrow<ArgumentNullException>();
            }
        }

        public class TheTimeEntriesProperty : BaseViewModelTests<TimelineViewModel>
        {

            protected override TimelineViewModel CreateViewModel()
                => new TimelineViewModel(DataSource);

            [Property]
            public void ReturnsTimeEntriesFromThisWeek(TimeEntry[] timeEntries)
            {
                ViewModel.TimeEntries.Clear();
                var observable = Observable.Return(timeEntries);
                DataSource.TimeEntries.GetAll().Returns(observable);

                ViewModel.Initialize().Wait();

                if (timeEntries.Length > 0)
                    ViewModel.TimeEntries
                             .ForEach(assertIsInThisWeek);
            }

            private void assertIsInThisWeek(ITimeEntry timeEnry)
            {
                var week = TimeSpan.FromDays(7);
                var delta = DateTime.Now - timeEnry.Start;
                delta.Should().BeLessThan(week);
            }

            [Property]
            public void TimeEntriesAreOrdered(TimeEntry[] timeEntries)
            {
                if (timeEntries.Length == 0) return;
                ViewModel.TimeEntries.Clear();
                var observable = Observable.Return(timeEntries);
                DataSource.TimeEntries.GetAll().Returns(observable);

                ViewModel.Initialize().Wait();

                for (int i = 1; i < ViewModel.TimeEntries.Count; i++)
                {
                    var start1 = ViewModel.TimeEntries[i - 1].Start;
                    var start2 = ViewModel.TimeEntries[i].Start;
                    start1.Should().BeBefore(start2);
                }
            }
        }

        public class TheProjectsProperty : BaseViewModelTests<TimelineViewModel>
        {
            protected override TimelineViewModel CreateViewModel()
                => new TimelineViewModel(DataSource);

            [Property]
            public void ReturnsAllProjects(Project[] projects)
            {
                ViewModel.Projects.Clear();
                var observable = Observable.Return(projects);
                DataSource.Projects.GetAll().Returns(observable);

                ViewModel.Initialize().Wait();

                ViewModel.Projects.Should().HaveCount(projects.Length);
                if (projects.Length > 0)
                    ViewModel.Projects.Should().Contain(projects);
            }
        }
    }
}
