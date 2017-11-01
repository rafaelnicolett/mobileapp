using System;
using Firebase.Analytics;
using Foundation;
using Toggl.Foundation.MvvmCross.Services;

namespace Toggl.Daneel.Services
{
    public sealed class FirebaseAnalyticsService : IAnalyticsService
    {
        private static readonly NSString[] pageTrackingKeys = { new NSString("ViewName") };

        public void TrackClosing(Type type)
        {
            Analytics.LogEvent($"Page closed", getParameters(type));
        }

        public void TrackOpening(Type type)
        {
            Analytics.LogEvent($"Page opened", getParameters(type));
        }

        private static NSDictionary<NSString, NSObject> getParameters(Type type)
        {
            var values = new NSObject[] { new NSString(type.Name) };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(
                pageTrackingKeys, values, pageTrackingKeys.Length);

            return parameters;
        }
    }
}
