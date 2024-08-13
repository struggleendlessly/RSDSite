using System;

namespace shared.Interfaces.Api
{
    public interface IApiService
    {
        Task<T> SendGetRequestAsync<T>(string endpoint, Dictionary<string, string> parameters);
        Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(TRequest request, string endpoint);
    }
}
