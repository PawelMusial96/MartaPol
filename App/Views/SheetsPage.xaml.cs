using MartaPol.App.ViewModels;

namespace MartaPol.App.Views;

public partial class SheetsPage : ContentPage
{
    public SheetsPage(SheetsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
