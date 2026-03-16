using FrontOfficeERP.API.DTOs;

namespace FrontOfficeERP.API.Services;

public interface IExcelCompareService
{
    Task<ExcelCompareResultDto> CompareAsync(ExcelCompareRequestDto request);
}
