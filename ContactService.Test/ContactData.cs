using ContactService.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Test
{
    public class ContactData
    {
        public ContactDto VALID = new ContactDto()
        {
            FirstName = "Yusuf",
            LastName = "Erden",
            Company = "ABC Ltd. Şti.",
            ContactDetails = new List<ContactDetailDto>()
            {
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.PHONE, ContactDetailContent = "0555 555 5555" },
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.EMAIL, ContactDetailContent = "example1@example.org" },
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.LOCATION, ContactDetailContent = "İstanbul" },
            }
        };

        public ContactDto UNVALID = new ContactDto()
        {
            FirstName = "Yusuf",
            Company = "DEF Ltd. Şti.",
            ContactDetails = new List<ContactDetailDto>()
            {
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.PHONE, ContactDetailContent = "0850 124 3322" },
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.EMAIL, ContactDetailContent = "example2@example.org" },
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.LOCATION, ContactDetailContent = "Ankara" },
            }
        };

        public List<ContactDto> DEMO = new List<ContactDto>() {
            new ContactDto()
            {
                FirstName = "Muhammed",
                LastName = "Erden",
                Company = "XYZ Ltd. Şti.",
                ContactDetails = new List<ContactDetailDto>()
                {
                    new ContactDetailDto(){ ContactDetailType = ContactDetailType.PHONE, ContactDetailContent = "0212 333 4455" },
                    new ContactDetailDto(){ ContactDetailType = ContactDetailType.EMAIL, ContactDetailContent = "example3@example.org" },
                    new ContactDetailDto(){ ContactDetailType = ContactDetailType.LOCATION, ContactDetailContent = "İzmir" },
                }
            },
            new ContactDto()
            {
                FirstName = "Mike",
                LastName = "Tyson",
                Company = "Tyson Lojistik ve Taşımacılık",
                ContactDetails = new List<ContactDetailDto>()
                {
                    new ContactDetailDto(){ ContactDetailType = ContactDetailType.PHONE, ContactDetailContent = "0545 123 4545" },
                    new ContactDetailDto(){ ContactDetailType = ContactDetailType.EMAIL, ContactDetailContent = "tyson@example.org" },
                    new ContactDetailDto(){ ContactDetailType = ContactDetailType.LOCATION, ContactDetailContent = "New York" },
                }
            }
        };
    }
}
