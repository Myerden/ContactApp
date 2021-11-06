using ReportService.Api.Dto;
using ReportService.Api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public Task GenerateReport(ReportDto reportDto)
        {


            throw new NotImplementedException();
        }
    }
}
