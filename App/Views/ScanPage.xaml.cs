using MartaPol.App.ViewModels;
using ZXing.Net.Maui;

namespace MartaPol.App.Views;

public partial class ScanPage : ContentPage
{
    private ScanViewModel Vm => (ScanViewModel)BindingContext;
    public ScanPage(ScanViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        Appearing += async (_, __) => await Vm.OnAppearingAsync();
    }

    private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var value = e.Results?.FirstOrDefault()?.Value;
        if (!string.IsNullOrWhiteSpace(value))
        {
            MainThread.BeginInvokeOnMainThread(() => Vm.HandleDetectedCode(value));
        }
    }
}
