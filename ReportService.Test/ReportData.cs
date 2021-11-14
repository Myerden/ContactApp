using ReportService.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Test
{
    public static class ReportData
    {
        public static List<ReportDto> DEMO = new List<ReportDto>() {
            new ReportDto()
            {
                Id = Guid.NewGuid(),
                ReportRequestDate = DateTime.Now.AddMinutes(-15),
                ReportStatus = Api.Model.ReportStatus.COMPLETED
            },
            new ReportDto()
            {
                Id = Guid.NewGuid(),
                ReportRequestDate = DateTime.Now.AddMinutes(-5),
                ReportStatus = Api.Model.ReportStatus.PREPARING
            },
            new ReportDto()
            {
                Id = Guid.NewGuid(),
                ReportRequestDate = DateTime.Now,
                ReportStatus = Api.Model.ReportStatus.PREPARING
            },
        };
    }
}
