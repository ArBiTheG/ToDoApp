using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ToDoApp.Controls;

public class LoadingBar : TemplatedControl
{
  public static readonly StyledProperty<string?> LoadingTextProperty =
    TextBlock.TextProperty.AddOwner<LoadingBar>();

    public string? LoadingText
    {
        get => GetValue(LoadingTextProperty);
        set => SetValue(LoadingTextProperty, value);
    }
}