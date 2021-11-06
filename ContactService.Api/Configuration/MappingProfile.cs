using AutoMapper;
using ContactService.Api.Dto;
using ContactService.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Api.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContactDetail, ContactDetailDto>().ReverseMap();
            CreateMap<Contact, ContactDto>().ReverseMap();
        }
    }
}
