using FrontOfficeApp.ViewModels;

namespace FrontOfficeApp.Views;

public partial class EmployeesPage : ContentPage
{
    private readonly EmployeesViewModel _vm;
    public EmployeesPage(EmployeesViewModel vm)
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
