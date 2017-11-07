﻿using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using Toggl.Daneel.Extensions;
using Toggl.Daneel.Presentation.Attributes;
using Toggl.Daneel.ViewSources;
using Toggl.Foundation.Autocomplete.Suggestions;
using Toggl.Foundation.MvvmCross.ViewModels;
using UIKit;

namespace Toggl.Daneel.ViewControllers
{
    [ModalCardPresentation]
    public sealed partial class SelectProjectViewController : MvxViewController<SelectProjectViewModel>
    {
        public SelectProjectViewController() : base(nameof(SelectProjectViewController), null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            var source = new SelectProjectTableViewSource(ProjectsTableView);
            ProjectsTableView.Source = source;
            ProjectsTableView.TableFooterView = new UIView();
            source.ToggleTasksCommand = new MvxCommand<ProjectSuggestion>(toggleTaskSuggestions);
            
            var bindingSet = this.CreateBindingSet<SelectProjectViewController, SelectProjectViewModel>();

            //Table view
            bindingSet.Bind(source).To(vm => vm.Suggestions);
            
            //Text
            bindingSet.Bind(TextField).To(vm => vm.Text);
            
            //Commands
            bindingSet.Bind(CloseButton).To(vm => vm.CloseCommand);
            bindingSet.Bind(source)
                      .For(s => s.SelectionChangedCommand)
                      .To(vm => vm.SelectProjectCommand);
            
            bindingSet.Apply();
        }

        private void toggleTaskSuggestions(ProjectSuggestion parameter)
        {
            var offset = ProjectsTableView.ContentOffset;
            var frameHeight = ProjectsTableView.Frame.Height;

            ViewModel.ToggleTaskSuggestionsCommand.Execute(parameter);

            ProjectsTableView.CorrectOffset(offset, frameHeight);
        }
    }
}
