using FrontOfficeERP.Models;
using OfficeOpenXml;

namespace FrontOfficeERP.Services;

public class ReportService
{
    public void GenerateEmployeeReport(List<Employee> employees, string outputFile = "EmployeeReport.xlsx")
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Employees");

        sheet.Cells[1, 1].Value = "Name";
        sheet.Cells[1, 2].Value = "Department";
        sheet.Cells[1, 3].Value = "Designation";

        var row = 2;
        foreach (var emp in employees)
        {
            sheet.Cells[row, 1].Value = emp.Name;
            sheet.Cells[row, 2].Value = emp.Department;
            sheet.Cells[row, 3].Value = emp.Designation;
            row++;
        }

        File.WriteAllBytes(outputFile, package.GetAsByteArray());
    }
}
