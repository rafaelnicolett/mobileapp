using System;
using System.Reactive.Linq;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using NSubstitute;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Foundation.Tests.Generators;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant.Models;
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

        public class TheTimeEntriesProperty
        {
            [Property]
            public Property ReturnsTimeEntriesFromThisWeek()
            {
                var generator = ViewModelGenerators.ForTimelineViewModel().ToArbitrary();
                return Prop.ForAll<TimelineViewModel>(generator, viewModel => 
                {
                    viewModel.Initialize().Wait();

                    if (viewModel.TimeEntries.Count > 0)
                        viewModel.TimeEntries
                                 .ForEach(assertIsInThisWeek);
                });
            }

            private void assertIsInThisWeek(TimelineTimeEntryViewModel timeEnry)
            {
                var week = TimeSpan.FromDays(7);
                var delta = DateTime.Now - timeEnry.Start;
                delta.Should().BeLessThan(week);
            }

            [Property]
            public Property TimeEntriesAreOrdered()
            {
                var generator = ViewModelGenerators.ForTimelineViewModel().ToArbitrary();
                return Prop.ForAll(generator, viewModel =>
                {
                    viewModel.Initialize().Wait();
                    if (viewModel.TimeEntries.Count == 0) return;

                    for (int i = 1; i < viewModel.TimeEntries.Count; i++)
                    {
                        var start1 = viewModel.TimeEntries[i - 1].Start;
                        var start2 = viewModel.TimeEntries[i].Start;
                        start1.Should().BeOnOrBefore(start2);
                    }
                });
            }
        }
    }
}
