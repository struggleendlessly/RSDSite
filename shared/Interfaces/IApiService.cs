using System;

namespace shared.Interfaces
{
    public interface IApiService
    {
        Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(TRequest request, string endpoint);
    }
}
