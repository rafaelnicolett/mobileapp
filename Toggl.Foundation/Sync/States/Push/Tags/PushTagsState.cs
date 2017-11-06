using System;
using Toggl.Foundation.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Sync.States
{
    internal sealed class PushTagsState : BasePushState<IDatabaseTag>
    {
        public PushTagsState(ITogglDatabase database)
            : base(database)
        {
        }

        protected override IRepository<IDatabaseTag> GetRepository(ITogglDatabase database)
            => database.Tags;

        protected override DateTimeOffset LastChange(IDatabaseTag entity)
            => entity.At;

        protected override IDatabaseTag CopyFrom(IDatabaseTag entity)
            => Tag.From(entity);
    }
}
