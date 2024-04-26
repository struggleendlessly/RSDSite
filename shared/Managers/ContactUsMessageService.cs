using shared.Data;
using shared.Interfaces;
using shared.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace shared.Managers
{
    public class ContactUsMessageService : IContactUsMessageService
    {
        private readonly ApplicationDbContext _dbContext;

        public ContactUsMessageService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ContactUsMessage>> GetContactUsMessages(string siteName)
        {
            return await _dbContext.ContactUsMessages
                .Include(x => x.Website)
                .Where(x => x.Website.Name == siteName)
                .OrderByDescending(x => x.Created)
                .ToListAsync();
        }

        public async Task<ContactUsMessage> CreateContactUsMessage(ContactUsMessage message)
        {
            _dbContext.ContactUsMessages.Add(message);
            await _dbContext.SaveChangesAsync();
            return message;
        }
    }
}
