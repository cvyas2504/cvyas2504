namespace FrontOfficeApp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private async void OnAboutClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("about");
    }
}
