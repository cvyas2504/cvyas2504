using FrontOfficeApp.ViewModels;

namespace FrontOfficeApp.Views;

public partial class ExcelComparePage : ContentPage
{
    public ExcelComparePage(ExcelCompareViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
