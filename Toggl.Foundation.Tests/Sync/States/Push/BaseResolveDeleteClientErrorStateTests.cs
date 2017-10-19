using System;
using System.Linq;
using System.Reactive.Linq;
using FluentAssertions;
using Toggl.Foundation.Sync;
using Toggl.Foundation.Sync.States;
using Toggl.Ultrawave.Exceptions;
using Xunit;

namespace Toggl.Foundation.Tests.Sync.States.Push
{
    public abstract class BaseResolveDeleteClientErrorStateTests<TModel>
    {
        [Theory]
        [MemberData(nameof(IgnoredClientExceptions))]
        public void ReturnTheDeleteLocallyTransition(ClientErrorException exception)
        {
            var entity = CreateEntity();
            var state = CreateState();

            var transition = state.Start((exception, entity)).Wait();
            var parameter = ((Transition<TModel>)transition).Parameter;

            transition.Result.Should().Be(state.DeleteLocally);
            parameter.Should().Be(entity);
        }

        protected abstract BaseResolveDeleteClientErrorState<TModel> CreateState();

        protected abstract TModel CreateEntity();

        protected static object[] NotIgnoredClientExcetions()
        {
            var ignoredErrors = IgnoredClientExceptions();
            var clientErros = new[]
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

            return (object[])clientErros.Where(error => !ignoredErrors.Contains(error));
        }

        protected abstract object[] IgnoredClientExceptions();
    }
}
