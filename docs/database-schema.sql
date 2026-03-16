PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Roles (
    RoleID INTEGER PRIMARY KEY AUTOINCREMENT,
    RoleName TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    RoleID INTEGER NOT NULL,
    IsActive INTEGER NOT NULL DEFAULT 1,
    CreatedDate TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

CREATE TABLE IF NOT EXISTS Permissions (
    PermissionID INTEGER PRIMARY KEY AUTOINCREMENT,
    RoleID INTEGER NOT NULL,
    ModuleName TEXT NOT NULL,
    CanCreate INTEGER NOT NULL DEFAULT 0,
    CanEdit INTEGER NOT NULL DEFAULT 0,
    CanDelete INTEGER NOT NULL DEFAULT 0,
    CanView INTEGER NOT NULL DEFAULT 0,
    CanExport INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    UNIQUE (RoleID, ModuleName)
);

CREATE TABLE IF NOT EXISTS Employees (
    EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeName TEXT NOT NULL,
    Department TEXT NOT NULL,
    Designation TEXT,
    Email TEXT,
    Phone TEXT,
    Status INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS DutyRoster (
    RosterID INTEGER PRIMARY KEY AUTOINCREMENT,
    EmployeeID INTEGER NOT NULL,
    DutyDate TEXT NOT NULL,
    ShiftType TEXT NOT NULL CHECK (ShiftType IN ('General', 'Morning', 'Evening', 'Night')),
    Department TEXT NOT NULL,
    CreatedBy INTEGER,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID),
    UNIQUE (EmployeeID, DutyDate)
);

CREATE TABLE IF NOT EXISTS ExcelCompareResults (
    CompareResultID INTEGER PRIMARY KEY AUTOINCREMENT,
    CompareBatchId TEXT NOT NULL,
    KeyColumn TEXT NOT NULL,
    File1Value TEXT,
    File2Value TEXT,
    MatchStatus TEXT NOT NULL,
    ComparedBy INTEGER,
    ComparedDate TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (ComparedBy) REFERENCES Users(UserID)
);

CREATE TABLE IF NOT EXISTS Reports (
    ReportID INTEGER PRIMARY KEY AUTOINCREMENT,
    ReportType TEXT NOT NULL,
    FilterJson TEXT,
    GeneratedBy INTEGER,
    GeneratedDate TEXT NOT NULL DEFAULT (datetime('now')),
    FOREIGN KEY (GeneratedBy) REFERENCES Users(UserID)
);

INSERT OR IGNORE INTO Roles (RoleID, RoleName) VALUES
(1, 'Admin'),
(2, 'Manager'),
(3, 'User');
