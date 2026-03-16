CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(300) NOT NULL,
    RoleID INT NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active',
    CreatedDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

CREATE TABLE Permissions (
    PermissionID INT IDENTITY(1,1) PRIMARY KEY,
    RoleID INT NOT NULL,
    ModuleName NVARCHAR(100) NOT NULL,
    CanView BIT NOT NULL DEFAULT 0,
    CanEdit BIT NOT NULL DEFAULT 0,
    CanDelete BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Permissions_Roles FOREIGN KEY (RoleID) REFERENCES Roles(RoleID),
    CONSTRAINT UQ_Permissions_Role_Module UNIQUE (RoleID, ModuleName)
);

CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeName NVARCHAR(150) NOT NULL,
    Department NVARCHAR(100) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active'
);

CREATE TABLE ShiftTypes (
    ShiftID INT IDENTITY(1,1) PRIMARY KEY,
    ShiftName NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE DutyRoster (
    RosterID INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeID INT NOT NULL,
    ShiftID INT NOT NULL,
    DutyDate DATE NOT NULL,
    CONSTRAINT FK_DutyRoster_Employees FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    CONSTRAINT FK_DutyRoster_ShiftTypes FOREIGN KEY (ShiftID) REFERENCES ShiftTypes(ShiftID),
    CONSTRAINT UQ_DutyRoster_Employee_DutyDate UNIQUE (EmployeeID, DutyDate)
);

CREATE TABLE ExcelCompareLogs (
    CompareID INT IDENTITY(1,1) PRIMARY KEY,
    File1Name NVARCHAR(255) NOT NULL,
    File2Name NVARCHAR(255) NOT NULL,
    ComparedDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ResultSummary NVARCHAR(MAX) NOT NULL
);

INSERT INTO ShiftTypes (ShiftName) VALUES ('General'),('Morning'),('Evening'),('Night');
