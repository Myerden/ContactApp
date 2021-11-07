using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactListingService;
using Grpc.Core;

namespace ContactService.Application.Services
{
    public class ContactService : ContactServiceGrpc.ContactServiceGrpcBase
    {
        public override Task<ContactListReply> ListAllContacts(ContactListRequest request, ServerCallContext context)
        {
            var response = new ContactListReply
            {
                
            };
            return Task.FromResult(response);
        }
    }
}
