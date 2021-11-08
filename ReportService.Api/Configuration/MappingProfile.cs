using AutoMapper;
using ReportService.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReportService.Api.Model;

namespace ReportService.Api.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Report, ReportDto>().ReverseMap();
        }
    }
}
