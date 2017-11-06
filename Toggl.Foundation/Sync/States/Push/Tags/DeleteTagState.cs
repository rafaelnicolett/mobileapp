using System;
using System.Reactive;
using Toggl.Foundation.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;
using Toggl.Ultrawave;

namespace Toggl.Foundation.Sync.States
{
    public class DeleteTagState : BaseDeleteEntityState<IDatabaseTag>
    {
        public DeleteTagState(ITogglApi api, IRepository<IDatabaseTag> repository) : base(api, repository)
        {
        }

        protected override IObservable<Unit> Delete(IDatabaseTag entity)
            => Api.Tags.Delete(entity);

        protected override IDatabaseTag CopyFrom(IDatabaseTag entity)
            => Tag.From(entity);
    }
}
