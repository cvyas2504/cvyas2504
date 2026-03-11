using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class LoginViewModel(IAuthService authService) : BaseViewModel
{
    [ObservableProperty] private string username = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string errorMessage = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        var user = await authService.LoginAsync(Username.Trim(), Password);
        IsBusy = false;
        if (user is null)
        {
            ErrorMessage = "Invalid username or password.";
            return;
        }

        await Shell.Current.GoToAsync("//dashboard");
    }
}
