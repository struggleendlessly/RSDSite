using System;

namespace shared.Interfaces
{
    public interface ISiteCreator
    {
        Task CreateSite(string siteName);
    }
}
