using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    private Window GetWindow() => TopLevel.GetTopLevel(this) as Window ?? throw new NullReferenceException("Invalid Owner");
    public MainView()
    {
        InitializeComponent();

        // This line is needed to make the previewer happy (the previewer plugin cannot handle the following line).
        if (Design.IsDesignMode) return;

        this.WhenActivated(action =>
                action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }
    private async Task DoShowDialogAsync(IInteractionContext<TaskEditorViewModel, TaskViewModel?> interaction)
    {
        var dialog = new TaskEditorWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<TaskViewModel?>(GetWindow());
        interaction.SetOutput(result);
    }
}
