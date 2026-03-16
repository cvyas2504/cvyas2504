using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Models;
using FrontOfficeApp.Services;
using System.Collections.ObjectModel;

namespace FrontOfficeApp.ViewModels;

public partial class UserManagementViewModel(IAuthService authService) : BaseViewModel
{
    public ObservableCollection<User> Users { get; } = new();

    [ObservableProperty] private string searchText = string.Empty;

    [RelayCommand]
    private async Task LoadAsync()
    {
        Users.Clear();
        var users = await authService.GetUsersAsync();
        foreach (var user in users.Where(x => string.IsNullOrWhiteSpace(SearchText) || x.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase)))
            Users.Add(user);
    }
}
