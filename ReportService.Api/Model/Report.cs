using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Model
{
    public class Report
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime ReportRequestDate { get; set; }
        [Required]
        public ReportStatus ReportStatus { get; set; }
        public string ReportPath { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public enum ReportStatus
    {
        PREPARING = 1,
        COMPLETED = 2,
        FAILED = 3
    }
}
