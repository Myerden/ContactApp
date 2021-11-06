using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Api.Dto
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Company { get; set; }
        [Required]
        public List<ContactDetailDto> ContactDetails { get; set; }
    }
}
