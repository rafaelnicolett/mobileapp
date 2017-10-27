using System;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.Ultrawave;
using Toggl.Ultrawave.Network;

namespace Toggl.Foundation.Login
{
    public sealed class ApiFactory : IApiFactory
    {
        public UserAgent UserAgent { get; }

        public ApiEnvironment Environment { get; }
        
        private Func<long, IObservable<IWorkspaceFeatureCollection>> getWorkspaceFeatures;

        public ApiFactory(ApiEnvironment apiEnvironment, UserAgent userAgent, Func<long, IObservable<IWorkspaceFeatureCollection>> getWorkspaceFeatures)
        {
            Ensure.Argument.IsNotNull(userAgent, nameof(userAgent));
            Ensure.Argument.IsNotNull(getWorkspaceFeatures, nameof(getWorkspaceFeatures));

            UserAgent = userAgent;
            Environment = apiEnvironment;
            this.getWorkspaceFeatures = getWorkspaceFeatures;
        }

        public ITogglApi CreateApiWith(Credentials credentials)
            => new TogglApi(new ApiConfiguration(Environment, credentials, UserAgent), getWorkspaceFeatures);
    }
}
