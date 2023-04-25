using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ToucanUI.ViewModels;

namespace ToucanUI.Views;

public partial class FooterView : UserControl
{
    public FooterView()
    {
        InitializeComponent();
        DataContext = new FooterViewModel();
    }
}