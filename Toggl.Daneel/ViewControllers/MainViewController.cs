﻿using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using MvvmCross.Plugins.Color;
using MvvmCross.Plugins.Color.iOS;
using MvvmCross.Plugins.Visibility;
using Toggl.Daneel.Combiners;
using Toggl.Daneel.Extensions;
using Toggl.Daneel.Views;
using Toggl.Foundation.MvvmCross.Converters;
using Toggl.Foundation.MvvmCross.Helper;
using Toggl.Foundation.MvvmCross.ViewModels;
using UIKit;

namespace Toggl.Daneel.ViewControllers
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class MainViewController : MvxViewController<MainViewModel>
    {
        private const float animationAngle = 0.1f;

        private readonly UIButton reportsButton = new UIButton(new CGRect(0, 0, 40, 40));
        private readonly UIButton settingsButton = new UIButton(new CGRect(0, 0, 40, 40));
        private readonly UIImageView titleImage = new UIImageView(UIImage.FromBundle("togglLogo"));

        public MainViewController()
            : base(nameof(MainViewController), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            prepareViews();

            var colorConverter = new MvxNativeColorValueConverter();
            var visibilityConverter = new MvxVisibilityValueConverter();
            var timeSpanConverter = new TimeSpanToDurationValueConverter();
            var invertedVisibilityConverter = new MvxInvertedVisibilityValueConverter();
            var projectTaskClientCombiner = new ProjectTaskClientValueCombiner(
                CurrentTimeEntryProjectTaskClientLabel.Font.CapHeight,
                Color.Main.CurrentTimeEntryClientColor.ToNativeColor(),
                true
            );

            var bindingSet = this.CreateBindingSet<MainViewController, MainViewModel>();

            //Commands
            bindingSet.Bind(settingsButton).To(vm => vm.OpenSettingsCommand);
            bindingSet.Bind(StopTimeEntryButton).To(vm => vm.StopTimeEntryCommand);
            bindingSet.Bind(StartTimeEntryButton).To(vm => vm.StartTimeEntryCommand);
            bindingSet.Bind(EditTimeEntryButton).To(vm => vm.EditTimeEntryCommand);
            bindingSet.Bind(MainPagedScrollView)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshCommand);

            bindingSet.Bind(CurrentTimeEntryCard)
                      .For(v => v.BindTap())
                      .To(vm => vm.EditTimeEntryCommand);

            //Visibility
            bindingSet.Bind(CurrentTimeEntryCard)
                      .For(v => v.BindVisibility())
                      .To(vm => vm.HasCurrentTimeEntry)
                      .WithConversion(visibilityConverter);

            bindingSet.Bind(StartTimeEntryButton)
                      .For(v => v.BindVisibility())
                      .To(vm => vm.HasCurrentTimeEntry)
                      .WithConversion(invertedVisibilityConverter);

            bindingSet.Bind(SpiderBroImageView)
                      .For(v => v.BindVisibility())
                      .To(vm => vm.SpiderIsVisible)
                      .WithConversion(visibilityConverter);

            bindingSet.Bind(SpiderHinge)
                     .For(v => v.BindVisibility())
                     .To(vm => vm.SpiderIsVisible)
                     .WithConversion(visibilityConverter);

            bindingSet.Bind(SyncIndicatorView)
                      .For(v => v.BindVisibility())
                      .To(vm => vm.IsSyncing)
                      .WithConversion(visibilityConverter);

            bindingSet.Bind(MainPagedScrollView)
                      .For(v => v.IsSyncing)
                      .To(vm => vm.IsSyncing);

            //Text
            bindingSet.Bind(CurrentTimeEntryDescriptionLabel).To(vm => vm.CurrentTimeEntryDescription);
            bindingSet.Bind(CurrentTimeEntryElapsedTimeLabel)
                      .To(vm => vm.CurrentTimeEntryElapsedTime)
                      .WithConversion(timeSpanConverter);

            bindingSet.Bind(CurrentTimeEntryProjectTaskClientLabel)
                      .For(v => v.AttributedText)
                      .ByCombining(projectTaskClientCombiner,
                                   v => v.CurrentTimeEntryProject,
                                   v => v.CurrentTimeEntryTask,
                                   v => v.CurrentTimeEntryClient,
                                   v => v.CurrentTimeEntryProjectColor);
            
            bindingSet.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationItem.TitleView = titleImage;
            NavigationItem.RightBarButtonItems = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace) { Width = -10 },
                new UIBarButtonItem(settingsButton),
                //new UIBarButtonItem(reportsButton)
            };
        }

        internal UIView GetContainerFor(UIViewController viewController)
        {
            if (viewController is SuggestionsViewController)
                return SuggestionsContainer;

            if (viewController is TimeEntriesLogViewController)
                return TimeEntriesLogContainer;

            throw new ArgumentOutOfRangeException(nameof(viewController), "Received unexpected ViewController type");
        }

        internal void AnimatePlayButton()
        {
            AnimationExtensions.Animate(
                Animation.Timings.EnterTiming,
                Animation.Curves.EaseOut,
                () => StartTimeEntryButton.Transform = CGAffineTransform.MakeScale(1f, 1f)
            );
        }

        private void prepareViews()
        {
            //Prevent bounces in UIScrollView
            AutomaticallyAdjustsScrollViewInsets = false;

            //Pull to refresh
            SyncIndicatorView.ContentMode = UIViewContentMode.ScaleAspectFit;
            MainPagedScrollView.SyncStateView = SyncStateView;
            MainPagedScrollView.SyncStateLabel = SyncStateLabel;
            MainPagedScrollView.ContentInset = new UIEdgeInsets(MainScrollView.SyncStateViewHeight * 2, 0, 0, 0);
            MainPagedScrollView.SetContentOffset(new CGPoint(0, 0), false);

            //Spider animation
            SpiderBroImageView.Layer.AnchorPoint = new CGPoint(0.5f, 0);
            animateSpider();

            //Card border
            CurrentTimeEntryCard.Layer.BorderWidth = 1;
            CurrentTimeEntryCard.Layer.CornerRadius = 8;
            CurrentTimeEntryCard.Layer.BorderColor = Color.TimeEntriesLog.ButtonBorder.ToNativeColor().CGColor;
            CurrentTimeEntryElapsedTimeLabel.Font = CurrentTimeEntryElapsedTimeLabel.Font.GetMonospacedDigitFont();

            //Hide play button for later animating it
            StartTimeEntryButton.Transform = CGAffineTransform.MakeScale(0.01f, 0.01f);

            //Prepare Navigation bar images
            reportsButton.SetImage(UIImage.FromBundle("icReports"), UIControlState.Normal);
            settingsButton.SetImage(UIImage.FromBundle("icSettings"), UIControlState.Normal);

            RunningEntryDescriptionFadeView.FadeLeft = true;
            RunningEntryDescriptionFadeView.FadeRight = true;
        }

        private void animateSpider()
        {
            SpiderBroImageView.Transform = CGAffineTransform.MakeRotation(-animationAngle);

            UIView.Animate(Animation.Timings.SpiderBro, 0, UIViewAnimationOptions.Autoreverse | UIViewAnimationOptions.Repeat, 
                () => SpiderBroImageView.Transform = CGAffineTransform.MakeRotation(animationAngle), animateSpider);
        }
    }
}
