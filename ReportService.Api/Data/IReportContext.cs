using ReportService.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Data
{
    public interface IReportContext
    {
        DbSet<Report> Reports { get; set; }
    }
}
