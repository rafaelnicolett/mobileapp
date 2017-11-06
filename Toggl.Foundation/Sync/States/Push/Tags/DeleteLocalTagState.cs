using System;
using System.Reactive;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Sync.States
{
    public sealed class DeleteLocalTagState : BaseDeleteLocalEntityState<IDatabaseTag>
    {
        public DeleteLocalTagState(IRepository<IDatabaseTag> repository)
            : base(repository)
        {
        }

        protected override IObservable<Unit> Delete(IDatabaseTag entity)
            => Repository.Delete(entity.Id);
    }
}
