using System;
using FsCheck.Xunit;
using Toggl.Foundation.Sync.States;
using Xunit;
using FluentAssertions;

namespace Toggl.Foundation.Tests.Sync.States
{
    public sealed class InvalidTransitionStateTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("          ")]
        public void ThrowsWhenTheConstructorArgumentIsNullOrEmpty(string message)
        {
            Action creatingState = () => new InvalidTransitionState(message);

            creatingState.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ThrowsInvalidOperationException()
        {
            Exception caughtException = null;
            var state = new InvalidTransitionState("Some message");

            var observable = state.Start();
            observable.Subscribe(_ => { }, (Exception exception) => caughtException = exception);

            caughtException.Should().NotBeNull();
            caughtException.Should().BeOfType<InvalidOperationException>();
        }

        [Property]
        public void ThrowsInvalidOperationExceptionWithASpecificMessage(string message)
        {
            Exception caughtException = null;
            var state = new InvalidTransitionState(message);

            var observable = state.Start();
            observable.Subscribe(_ => { }, (Exception exception) => caughtException = exception);

            caughtException.Message.Should().Be($"Invalid transition: {message}");
        }
    }
}
