using MartaPol.App.ViewModels;

namespace MartaPol.App.Views;

public partial class SheetDetailPage : ContentPage
{
    public SheetDetailPage(SheetDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
