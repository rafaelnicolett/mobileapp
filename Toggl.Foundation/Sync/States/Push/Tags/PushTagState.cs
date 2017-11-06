using System;
using Toggl.Foundation.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Sync.States
{
    internal sealed class PushTagState : BasePushState<IDatabaseTag>
    {
        public PushTagState(ITogglDatabase database)
            : base(database)
        {
        }

        protected override IRepository<IDatabaseTag> GetRepository(IDatabaseTag database)
            => database.Tags;

        protected override DateTimeOffset LastChange(IDatabaseTag entity)
            => entity.At;

        protected override IDatabaseTag CopyFrom(IDatabaseTag entity)
            => Tag.From(entity);
    }
}
