using System;
using MvvmCross.Core.ViewModels;

namespace Toggl.Foundation.MvvmCross.Services
{
    public interface IAnalyticsService
    {
        void TrackOpening(Type type);

        void TrackClosing(Type type);
    }
}
