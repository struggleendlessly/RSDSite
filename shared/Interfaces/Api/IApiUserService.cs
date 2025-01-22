using shared.Data.Entities;

namespace shared.Interfaces.Api
{
    public interface IApiUserService
    {
        Task<User> GetOrCreateAsync();
    }
}
