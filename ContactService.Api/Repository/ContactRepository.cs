using ContactService.Api.Data;
using ContactService.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactService.Api.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactContext _dbContext;

        public ContactRepository(ContactContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Create(Contact contact)
        {
            contact.CreatedAt = DateTime.Now;
            await _dbContext.AddAsync(contact);
            await _dbContext.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<bool> Update(Contact contact)
        {
            try
            {
                contact.UpdatedAt = DateTime.Now;
                _dbContext.Entry(contact).State = EntityState.Modified;
                _dbContext.Entry(contact).Property(c => c.CreatedAt).IsModified = false;
                _dbContext.Entry(contact).Collection(c => c.ContactDetails).IsModified = true;

                var num = await _dbContext.SaveChangesAsync();
                return num > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var contact = await Get(id);
                _dbContext.Contacts.Remove(contact);
                var num = await _dbContext.SaveChangesAsync();
                return num > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Contact>> Get()
        {
            return await _dbContext.Contacts.ToListAsync();
        }

        public async Task<Contact> Get(Guid id)
        {
            return await _dbContext.Contacts.Where(c => c.Id == id).Include(c => c.ContactDetails).FirstOrDefaultAsync();
        }

    }
}
