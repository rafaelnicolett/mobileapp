using System;
using System.Reactive.Linq;
using Toggl.Ultrawave.Exceptions;

namespace Toggl.Foundation.Sync.States
{
    public abstract class BaseResolveDeleteClientErrorState<TModel>
    {
        public StateResult<TModel> DeleteLocally { get; } = new StateResult<TModel>();
        public StateResult<TModel> MarkUnsyncable { get; } = new StateResult<TModel>();

        public IObservable<ITransition> Start((ClientErrorException Reason, TModel Entity) parameter)
            => IgnoreTheError(parameter.Reason)
                ? Observable.Return(DeleteLocally.Transition(parameter.Entity))
                : Observable.Return(MarkUnsyncable.Transition(parameter.Entity));

        protected abstract bool IgnoreTheError(ClientErrorException error);
    }
}
