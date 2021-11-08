using ReportService.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Data
{
    public class ReportContext : DbContext, IReportContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Report>().Property(cd => cd.ReportStatus).HasConversion<int>().IsRequired();

            modelBuilder.Entity<Report>().Property(cd => cd.ReportPath).HasMaxLength(500);
        }
    }
}
