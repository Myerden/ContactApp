using ContactService.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactService.Test
{
    public static class ContactData
    {
        public static ContactDto VALID = new ContactDto()
        {
            FirstName = "Yusuf",
            LastName = "Erden",
            Company = "ABC Ltd. Şti.",
            ContactDetails = new List<ContactDetailDto>()
            {
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.PHONE, ContactDetailContent = "0555 555 5555" },
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.EMAIL, ContactDetailContent = "example@example.org" },
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.LOCATION, ContactDetailContent = "Istanbul" },
            }
        };

        public static ContactDto UNVALID = new ContactDto()
        {
            FirstName = "Yusuf",
            Company = "ABC Ltd. Şti.",
            ContactDetails = new List<ContactDetailDto>()
            {
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.PHONE, ContactDetailContent = "0555 555 5555" },
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.EMAIL, ContactDetailContent = "example@example.org" },
                new ContactDetailDto(){ ContactDetailType = ContactDetailType.LOCATION, ContactDetailContent = "Istanbul" },
            }
        };
    }
}
