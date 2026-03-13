namespace FrontOfficeApp.Models;

public enum UserRole
{
    Admin,
    Manager,
    Operator,
    Viewer
}

public enum ModuleType
{
    Dashboard,
    Employee,
    DutyRoster,
    ExcelCompare,
    Reports,
    UserManagement
}

public enum ShiftType
{
    G,
    M,
    E,
    N,
    O
}

public enum MatchStatus
{
    Match,
    Difference,
    MissingInFile1,
    MissingInFile2,
    Mismatch
}
