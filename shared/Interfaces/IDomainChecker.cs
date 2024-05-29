using System;

namespace shared.Interfaces
{
    public interface IDomainChecker
    {
        Task<bool> IsDomainWorkingAsync(string domain);
    }
}
