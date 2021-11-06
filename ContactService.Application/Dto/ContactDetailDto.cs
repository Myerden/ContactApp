using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Application.Dto
{
    public class ContactDetailDto
    {
        public Guid Id { get; set; }
        [Required]
        public ContactDetailType ContactDetailType { get; set; }
        [Required]
        public string ContactDetailContent { get; set; }
    }

    public enum ContactDetailType
    {
        PHONE = 1,
        EMAIL = 2,
        LOCATION = 3
    }
}
