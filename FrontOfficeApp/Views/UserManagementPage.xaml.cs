using FrontOfficeApp.ViewModels;

namespace FrontOfficeApp.Views;

public partial class UserManagementPage : ContentPage
{
    public UserManagementPage(UserManagementViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _ = viewModel.LoadCommand.ExecuteAsync(null);
    }
}
