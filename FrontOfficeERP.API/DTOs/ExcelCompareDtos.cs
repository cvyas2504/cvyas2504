namespace FrontOfficeERP.API.DTOs;

public record ExcelCompareRequestDto(string File1Name, string File2Name, List<Dictionary<string, string>> File1Rows, List<Dictionary<string, string>> File2Rows);
public record ExcelDifferenceDto(string Key, string ChangeType, string Details);
public record ExcelCompareResultDto(string Summary, List<ExcelDifferenceDto> Differences);
