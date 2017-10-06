using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using Toggl.Daneel.Extensions;
using Toggl.Foundation.MvvmCross.Converters;
using Toggl.Foundation.MvvmCross.Helper;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.PrimeRadiant.Models;
using UIKit;

namespace Toggl.Daneel.Views
{
    public sealed partial class TimeEntryCardView : MvxView
    {
        public IMvxCommand StopCommand { get; set; }

        public IDatabaseTimeEntry CurrentlyRunningTimeEntry
        {
            get => DataContext as IDatabaseTimeEntry;
            set
            {
                var isHiding = DataContext != null && value == null;
                if (isHiding)
                {
                    AnimationExtensions.Animate(
                        Animation.Timings.LeaveTimingFaster,
                        Animation.Curves.EaseIn,
                        () => StopButton.Transform = CGAffineTransform.MakeScale(0.01f, 0.01f)
                    );

                    AnimationExtensions.Animate(
                        Animation.Timings.LeaveTiming,
                        Animation.Curves.CardOutCurve,
                        () => Transform = CGAffineTransform.MakeTranslation(0, Bounds.Height)
                    );
                }

                var isShowing = DataContext == null && value != null;
                if (isShowing)
                {
                    AnimationExtensions.Animate(
                        Animation.Timings.LeaveTimingFaster,
                        Animation.Curves.EaseIn,
                        () => StopButton.Transform = CGAffineTransform.MakeScale(1f, 1f)
                    );

                    AnimationExtensions.Animate(
                        Animation.Timings.LeaveTiming,
                        Animation.Curves.CardOutCurve,
                        () => Transform = CGAffineTransform.MakeTranslation(0, 0)
                    );
                }

                DataContext = value;
            }
        }

        public TimeEntryCardView(IntPtr handle)
            : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            Transform = CGAffineTransform.MakeTranslation(0, Bounds.Height);
            StopButton.Transform = CGAffineTransform.MakeScale(0.01f, 0.01f);

            this.DelayBind(() =>
            {
                var bindingSet = this.CreateBindingSet<TimeEntryCardView, CurrentlyRunningTimeEntryViewModel>();

                bindingSet.Bind(DescriptionLabel).To(vm => vm.Description);
                bindingSet.Bind(TimerLabel)
                          .To(vm => vm.ElapsedTime)
                          .WithConversion(new TimeSpanToDurationValueConverter());

                bindingSet.Apply();
            });
        }
    }
}
