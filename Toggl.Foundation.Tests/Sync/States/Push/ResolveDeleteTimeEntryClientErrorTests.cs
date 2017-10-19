using System;
using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using Toggl.Foundation.Models;
using Toggl.Foundation.Sync;
using Toggl.Foundation.Sync.States;
using Toggl.PrimeRadiant.Models;
using Toggl.Ultrawave.Exceptions;
using Xunit;

namespace Toggl.Foundation.Tests.Sync.States
{
    public sealed class ResolveDeleteTimeEntryClientErrorTests
    {
        [Theory]
        [MemberData(nameof(ignoredClientExceptions))]
        public void ReturnTheDeleteLocallyTransition(ClientErrorException exception)
        {
            var entity = TimeEntry.Dirty(new Ultrawave.Models.TimeEntry { Id = 123 });
            var state = new ResolveDeleteTimeEntryClientErrorState();

            var transition = state.Start((exception, entity)).Wait();
            var parameter = ((Transition<IDatabaseTimeEntry>)transition).Parameter;

            transition.Result.Should().Be(state.DeleteLocally);
            parameter.ShouldBeEquivalentTo(entity, options => options.IncludingProperties());
        }

        [Theory]
        [MemberData(nameof(notIgnoredClientExcetions))]
        public void ReturnTheMarkUnsyncableTransition(ClientErrorException exception)
        {
            var entity = TimeEntry.Dirty(new Ultrawave.Models.TimeEntry { Id = 123 });
            var state = new ResolveDeleteTimeEntryClientErrorState();

            var transition = state.Start((exception, entity)).Wait();
            var parameter = ((Transition<IDatabaseTimeEntry>)transition).Parameter;

            transition.Result.Should().Be(state.MarkUnsyncable);
            parameter.ShouldBeEquivalentTo(entity, options => options.IncludingProperties());
        }

        private static object[] notIgnoredClientExcetions()
            => new object[0];

        private static object[] ignoredClientExceptions()
            => new[]
            {
                new object[] { new BadRequestException() },
                new object[] { new UnauthorizedException() },
                new object[] { new PaymentRequiredException() },
                new object[] { new ForbiddenException() },
                new object[] { new NotFoundException() },
                new object[] { new ApiDeprecatedException() },
                new object[] { new RequestEntityTooLargeException() },
                new object[] { new ClientDeprecatedException() },
                new object[] { new TooManyRequestsException() }
            };

    }
}
