using System;
using System.Reactive.Linq;
using Toggl.Foundation.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;
using Toggl.Ultrawave;

namespace Toggl.Foundation.Sync.States
{
    internal sealed class UpdateTagState : BaseUpdateEntityState<IDatabaseTag>
    {
        public UpdateTagState(ITogglApi api, IRepository<IDatabaseTag> repository) : base(api, repository)
        {
        }

        protected override bool HasChanged(IDatabaseTag original, IDatabaseTag current)
            => original.At < current.At;

        protected override IObservable<IDatabaseTag> Update(IDatabaseTag entity)
            => Api.Tags.Update(entity).Select(Tag.Clean);

        protected override IDatabaseTag CopyFrom(IDatabaseTag entity)
            => Tag.From(entity);
    }
}
