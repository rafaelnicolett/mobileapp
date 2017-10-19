using Toggl.PrimeRadiant.Models;
using Toggl.Ultrawave.Exceptions;

namespace Toggl.Foundation.Sync.States
{
    public sealed class ResolveDeleteTimeEntryClientErrorState : BaseResolveDeleteClientErrorState<IDatabaseTimeEntry>
    {
        protected override bool IgnoreTheError(ClientErrorException error)
            => true;
    }
}
