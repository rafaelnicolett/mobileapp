﻿using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using Toggl.Foundation.Autocomplete.Suggestions;
using UIKit;

namespace Toggl.Daneel.Views
{
    public sealed partial class TaskSuggestionViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString(nameof(TaskSuggestionViewCell));
        public static readonly UINib Nib;

        static TaskSuggestionViewCell()
        {
            Nib = UINib.FromName(nameof(TaskSuggestionViewCell), NSBundle.MainBundle);
        }

        protected TaskSuggestionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            FadeView.FadeRight = true;

            this.DelayBind(() =>
            {
                var bindingSet = this.CreateBindingSet<TaskSuggestionViewCell, TaskSuggestion>();

                bindingSet.Bind(TaskNameLabel).To(vm => vm.Name);

                bindingSet.Apply();
            });
        }
    }
}
