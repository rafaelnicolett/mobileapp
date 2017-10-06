// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.Views
{
    [Register ("TimeEntryCardView")]
    partial class TimeEntryCardView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ArrowUpButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton StopButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimerLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ArrowUpButton != null) {
                ArrowUpButton.Dispose ();
                ArrowUpButton = null;
            }

            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (StopButton != null) {
                StopButton.Dispose ();
                StopButton = null;
            }

            if (TimerLabel != null) {
                TimerLabel.Dispose ();
                TimerLabel = null;
            }
        }
    }
}