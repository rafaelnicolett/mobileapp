﻿using System;
using FsCheck.Xunit;
using Toggl.Foundation.Sync.States;
using Xunit;
using FluentAssertions;
using FsCheck;

namespace Toggl.Foundation.Tests.Sync.States
{
    public sealed class InvalidTransitionStateTests
    {
        [Fact]
        public void ThrowsWhenTheConstructorArgumentIsNull()
        {
            Action creatingState = () => new InvalidTransitionState(null);

            creatingState.ShouldThrow<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("          ")]
        public void ThrowsWhenTheConstructorArgumentIsEmpty(string message)
        {
            Action creatingState = () => new InvalidTransitionState(message);

            creatingState.ShouldThrow<ArgumentException>();
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
        public void ThrowsInvalidOperationExceptionWithASpecificMessage(NonEmptyString message)
        {
            Exception caughtException = null;
            var state = new InvalidTransitionState(message.Get);

            var observable = state.Start();
            observable.Subscribe(_ => { }, (Exception exception) => caughtException = exception);

            caughtException.Message.Should().Be($"Invalid transition: {message}");
        }
    }
}
