using MassTransit;
using ReportService.Api.Dto;
using ReportService.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Consumer
{
    public class ReportRequestConsumer : IConsumer<ReportDto>
    {
        private readonly IReportService _reportService;

        public ReportRequestConsumer(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task Consume(ConsumeContext<ReportDto> context)
        {
            await _reportService.GenerateReport(context.Message);
        }
    }
}
