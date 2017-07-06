using System;
using System.Reactive.Linq;
using FluentAssertions;
using FsCheck.Xunit;
using NSubstitute;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Ultrawave;
using Toggl.Ultrawave.Models;
using Xunit;

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
            public void ReturnsAllTimeEntries(TimeEntry[] timeEntries)
            {
                ViewModel.TimeEntries.Clear();
                var observable = Observable.Return(timeEntries);
                DataSource.TimeEntries.GetAll().Returns(observable);

                ViewModel.Initialize().Wait();

                ViewModel.TimeEntries.Should().HaveCount(timeEntries.Length);
                if (timeEntries.Length > 0)
                    ViewModel.TimeEntries.Should().Contain(timeEntries);
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
