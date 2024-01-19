using Avalonia.Controls;
using PzomPatch.ViewModels;

namespace PzomPatch.Views;

public partial class MainView : Window
{
    public MainView()
    {
    }

    public MainView(MainViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}