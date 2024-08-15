using shared.Data;
using shared.Interfaces;
using shared.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace shared.Managers
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<User?> GetAsync(string idpName, Guid idpUserId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
                (idpName.Equals(StaticStrings.MSAL_IDP_Facebook, StringComparison.InvariantCultureIgnoreCase) && u.FacebookId == idpUserId) ||
                (idpName.Equals(StaticStrings.MSAL_IDP_Google, StringComparison.InvariantCultureIgnoreCase) && u.GoogleId == idpUserId) ||
                (idpName.Equals(StaticStrings.MSAL_IDP_Microsoft, StringComparison.InvariantCultureIgnoreCase) && u.MicrosoftId == idpUserId));

            return user;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetOrCreateAsync(string idpName, Guid idpUserId)
        {
            var user = await GetAsync(idpName, idpUserId);
            if (user is null)
            {
                user = new User();

                var idpMapping = new Dictionary<string, Action>
                {
                    { StaticStrings.MSAL_IDP_Facebook, () => user.FacebookId = idpUserId },
                    { StaticStrings.MSAL_IDP_Google, () => user.GoogleId = idpUserId },
                    { StaticStrings.MSAL_IDP_Microsoft, () => user.MicrosoftId = idpUserId }
                };

                if (idpMapping.TryGetValue(idpName, out var setIdAction))
                {
                    setIdAction();
                }
                else
                {
                    throw new ArgumentException("Unsupported identity provider");
                }

                user = await CreateAsync(user);
            }

            return user;
        }
    }
}
