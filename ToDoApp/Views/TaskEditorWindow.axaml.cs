using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using ToDoApp.ViewModels;

namespace ToDoApp.Views
{
    public partial class TaskEditorWindow : ReactiveWindow<TaskEditorViewModel>
    {
        public TaskEditorWindow()
        {
            InitializeComponent();
            // This line is needed to make the previewer happy (the previewer plugin cannot handle the following line).
            if (Design.IsDesignMode) return;

            this.WhenActivated(action => action(ViewModel!.SubmitCommand.Subscribe(Close)));
        }
    }
}
