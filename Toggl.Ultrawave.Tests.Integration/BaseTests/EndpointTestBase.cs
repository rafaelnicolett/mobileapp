using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Toggl.Multivac.Models;
using Toggl.Ultrawave.Network;
using Toggl.Ultrawave.Tests.Integration.Helper;

namespace Toggl.Ultrawave.Tests.Integration.BaseTests
{
    public abstract class EndpointTestBase
    {
        protected async Task<(ITogglApi togglClient, IUser user)> SetupTestUser(Func<long, IObservable<IWorkspaceFeatureCollection>> getWorkspaceFeatures = null)
        {
            var credentials = await User.Create();
            var togglApi = TogglApiWith(credentials, getWorkspaceFeatures ?? NoFeatures);
            var user = await togglApi.User.Get();

            return (togglApi, user);
        }

        protected ITogglApi TogglApiWith(Credentials credentials, Func<long, IObservable<IWorkspaceFeatureCollection>> getWorkspaceFeatures)
            => new TogglApi(configurationFor(credentials), getWorkspaceFeatures);

        private ApiConfiguration configurationFor(Credentials credentials)
            => new ApiConfiguration(ApiEnvironment.Staging, credentials, Configuration.UserAgent);

        protected IObservable<IWorkspaceFeatureCollection> NoFeatures(long workspaceId)
            => Observable.Return(new Ultrawave.Models.WorkspaceFeatureCollection
            {
                WorkspaceId = workspaceId,
                Features = new IWorkspaceFeature[0]
            });
    }
}
