using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Api.Model
{
    public class ContactDetail
    {
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
