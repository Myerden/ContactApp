﻿using ReportService.Api.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Dto
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public DateTime ReportRequestDate { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public string ReportPath { get; set; }
    }
}
