using ContactService.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Api.Repository
{
    public interface IContactRepository
    {
        public Task<Guid> Create(Contact contact);

        public Task<bool> Update(Contact contact);

        public Task<bool> Delete(Guid id);

        public Task<IEnumerable<Contact>> Get();

        public Task<Contact> Get(Guid id);

        public Task<bool> Exists(Guid id);

    }
}
