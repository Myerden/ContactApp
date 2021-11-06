using ContactService.Api.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Api.Dto
{
    public class ContactDetailDto
    {
        public Guid Id { get; set; }
        [Required]
        public ContactDetailType ContactDetailType { get; set; }
        [Required]
        public string ContactDetailContent { get; set; }
    }

}
