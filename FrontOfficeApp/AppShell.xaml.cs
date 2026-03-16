namespace FrontOfficeApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("excel", typeof(Views.ExcelComparePage));
        Routing.RegisterRoute("employees", typeof(Views.EmployeesPage));
        Routing.RegisterRoute("users", typeof(Views.UserManagementPage));
        Routing.RegisterRoute("roster", typeof(Views.DutyRosterPage));
        Routing.RegisterRoute("reports", typeof(Views.ReportsPage));
        Routing.RegisterRoute("settings", typeof(Views.SettingsPage));
        Routing.RegisterRoute("about", typeof(Views.AboutPage));
    }
}
