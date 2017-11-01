using System;
using Foundation;
using UIKit;

namespace Toggl.Daneel.Views
{
    [Register(nameof(TextViewWithPlaceholder))]
    public partial class TextViewWithPlaceholder : UIView, IUITextViewDelegate
    {
        public TextViewWithPlaceholder(IntPtr handle)
            : base(handle)
        {
        }

        public UITextView TextView => InnerTextView;

        public string Placeholder
        {
            get => PlaceholderLabel.Text;
            set => PlaceholderLabel.Text = value;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            InnerTextView.Delegate = this;
        }

        [Export("textView:shouldChangeTextInRange:replacementText:")]
        public bool ShouldChangeText(UITextView textView, NSRange range, string text)
        {
            if (text == "\n")
            {
                textView.ResignFirstResponder();
                return false;
            }
            return true;
        }

        [Export("textViewDidChange:")]
        public void Changed(UITextView textView)
            => textView.Text = textView.Text.Replace('\n', ' ');
    }
}
