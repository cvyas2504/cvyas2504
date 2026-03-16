namespace FrontOfficeERP.API.DTOs;

public record DutyRosterCreateDto(int EmployeeId, int ShiftId, DateTime DutyDate);
public record DutyRosterDto(int RosterId, int EmployeeId, string EmployeeName, int ShiftId, string ShiftName, DateTime DutyDate);
