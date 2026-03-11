# Front Office ERP - Step by Step Code Blueprint

This guide provides a practical ERP starter structure used in real systems, including modules, reports, SQLite database, and an Excel compare utility.

## 1) ERP Architecture

```text
FrontOfficeERP
│
├── Models
├── ViewModels
├── Views
├── Services
├── Database
├── Reports
├── Helpers
├── Resources
```

Layer flow:

```text
UI (Pages)
   ↓
ViewModel
   ↓
Services
   ↓
Database
```

## 2) Full Database Design (SQLite)

Database file: `erp.db3`

Tables included:

- Users (`Id`, `Username`, `Password`, `Role`)
- Employees (`Id`, `Name`, `Department`, `Designation`, `Shift`, `Phone`, `Email`, `JoinDate`)
- Departments (`Id`, `DepartmentName`)
- DutyRoster (`Id`, `EmployeeId`, `Date`, `Shift`)
- Attendance (`Id`, `EmployeeId`, `Date`, `CheckIn`, `CheckOut`)
- Visitors (`Id`, `Name`, `Purpose`, `VisitDate`, `EmployeeToMeet`)
- ExcelCompareReports (`Id`, `File1`, `File2`, `CompareDate`, `Result`)

See SQL initializer in `FrontOfficeERP/Database/ERPDatabase.cs`.

## 3) Project Structure

```text
FrontOfficeERP
│
├── Models
│   ├── Employee.cs
│   ├── DutyRoster.cs
│   ├── Attendance.cs
│   ├── Visitor.cs
│   ├── User.cs
│   └── CompareResult.cs
│
├── Services
│   ├── DatabaseService.cs
│   ├── ExcelCompareService.cs
│   └── ReportService.cs
│
├── Views
│   ├── LoginPage.xaml
│   ├── DashboardPage.xaml
│   ├── EmployeePage.xaml
│   ├── DutyRosterPage.xaml
│   └── ReportsPage.xaml
│
├── ViewModels
│   └── EmployeeViewModel.cs
│
├── Database
│   └── ERPDatabase.cs
│
└── Program.cs
```

## 4) Main Modules

1. Login (Username, Password, Role)
2. Dashboard (employee count, today roster, visitors, reports)
3. Employee Management (add/edit/delete/search)
4. Duty Roster (daily/weekly scheduling)
5. Excel Compare Tool (upload and compare two files)
6. Reports Module (Employee, Duty Roster, Attendance, Excel Compare)

## 5) Sidebar Navigation UI (MAUI)

The dashboard page includes a sidebar with:

- Dashboard
- Employees
- Duty Roster
- Attendance
- Visitors
- Excel Compare
- Reports
- Settings

And footer text:

- Developed by Chetan

See `FrontOfficeERP/Views/DashboardPage.xaml`.

## 6) Report System (EPPlus)

`ReportService.GenerateEmployeeReport(...)` creates an Excel file with employee details.

Output example: `EmployeeReport.xlsx`

## 7) Excel Compare Tool

`ExcelCompareService.Compare(file1, file2)` compares each cell and returns differences as:

- Row
- Column
- File1Value
- File2Value

Use these results to export `ExcelCompareReport.xlsx`.

## 8) Sample Core Code Flow

`Program.cs` demonstrates the ERP startup flow:

1. Initialize SQLite schema
2. Create database/report services
3. Add an employee
4. Load employees
5. Generate employee report

## 9) Security Roles

Basic role idea:

- Admin → full access
- Manager → reports + roster
- User → view only

Use the `Users` table role value to enforce page or action permissions.

## 10) Final ERP Feature Checklist

- ✅ Login system
- ✅ Dashboard
- ✅ Employee management
- ✅ Duty roster scheduling
- ✅ Attendance
- ✅ Visitor management
- ✅ Excel comparison tool
- ✅ Reports export
- ✅ SQLite database
- ✅ Sidebar UI
- ✅ Excel reporting

---

**Developed by Chetan**
