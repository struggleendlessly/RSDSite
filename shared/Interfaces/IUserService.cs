using shared.Data.Entities;

namespace shared.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetAsync(string idpName, Guid idpUserId);
        Task<User> CreateAsync(User user);
        Task<User> GetOrCreateAsync(string idpName, Guid idpUserId);
    }
}
