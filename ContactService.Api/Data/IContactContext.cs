using ContactService.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Api.Data
{
    public interface IContactContext
    {
        DbSet<Contact> Contacts { get; set; }
    }
}
