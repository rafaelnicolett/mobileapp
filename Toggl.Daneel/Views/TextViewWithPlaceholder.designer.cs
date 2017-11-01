// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Toggl.Daneel.Views
{
	partial class TextViewWithPlaceholder
	{
		[Outlet]
		UIKit.UITextView InnerTextView { get; set; }

		[Outlet]
		UIKit.UILabel PlaceholderLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (InnerTextView != null) {
				InnerTextView.Dispose ();
				InnerTextView = null;
			}

            if (PlaceholderLabel != null) {
                PlaceholderLabel.Dispose ();
                PlaceholderLabel = null;
			}
		}
	}
}
