using ReportService.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Services
{
    public interface IReportService
    {
        Task GenerateReport(ReportDto reportDto);
    }
}
