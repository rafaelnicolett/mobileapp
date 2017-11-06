﻿using System;
using NSubstitute;
using Toggl.Foundation.Models;
using Toggl.Foundation.Sync;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Tests.Sync.States.Push
{
    public sealed class UnsyncableTagStateTests : BaseUnsyncableEntityStateTests
    {
        public UnsyncableTagStateTests()
            : base(new TheStartMethod())
        {
        }

        private sealed class TheStartMethod : TheStartMethod<IDatabaseTag>
        {
            protected override BaseUnsyncableEntityState<IDatabaseTag> CreateState(IRepository<IDatabaseTag> repository)
                => new UnsyncableTagState(repository);

            protected override IDatabaseTag CreateDirtyEntity()
                => Tag.Dirty(new Ultrawave.Models.Tag { Name = Guid.NewGuid().ToString() });
        }
    }
}
