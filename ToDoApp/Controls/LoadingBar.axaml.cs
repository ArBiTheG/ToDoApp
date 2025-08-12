using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace ToDoApp.Controls;

public partial class LoadingBar : UserControl
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LoadingBar, string>(nameof(Text), defaultValue: "", defaultBindingMode: BindingMode.TwoWay);
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public LoadingBar()
    {
        InitializeComponent();
    }
}