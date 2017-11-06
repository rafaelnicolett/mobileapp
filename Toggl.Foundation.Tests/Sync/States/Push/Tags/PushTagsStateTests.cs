using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using NSubstitute;
using Toggl.Foundation.Models;
using Toggl.Foundation.Sync.States;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Tests.Sync.States.Push
{
    public class PushTagsStateTests : BasePushStateTests
    {
        public PushTagsStateTests()
            : base(new TheStartMethod())
        {
        }

        private class TheStartMethod : TheStartMethod<IDatabaseTag>
        {
            protected override BasePushState<IDatabaseTag> CreateState(ITogglDatabase database)
                => new PushTagsState(database);

            protected override IDatabaseTag CreateUnsyncedEntity(DateTimeOffset lastUpdate = default(DateTimeOffset))
                => Tag.Dirty(new Ultrawave.Models.Tag { At = lastUpdate });

            protected override void SetupRepositoryToReturn(ITogglDatabase database, IDatabaseTag[] entities)
            {
                database.Tags.GetAll(Arg.Any<Func<IDatabaseTag, bool>>()).Returns(Observable.Return(entities));
            }

            protected override void SetupRepositoryToThrow(ITogglDatabase database)
            {
                database.Tags.GetAll(Arg.Any<Func<IDatabaseTag, bool>>()).Returns(_ => { throw new TestException(); });
            }
        }
    }
}
