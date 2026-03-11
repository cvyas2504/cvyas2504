using FrontOfficeApp.ViewModels;

namespace FrontOfficeApp.Views;

public partial class DutyRosterPage : ContentPage
{
    private readonly DutyRosterViewModel _vm;
    public DutyRosterPage(DutyRosterViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadCommand.ExecuteAsync(null);
    }
}
