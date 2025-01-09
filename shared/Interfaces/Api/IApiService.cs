using System;

namespace shared.Interfaces.Api
{
    public interface IApiService
    {
        Task<T> SendGetRequestAsync<T>(string endpoint, Dictionary<string, string> parameters);
        Task<TResult> SendPostRequestAsync<TRequest, TResult>(string endpoint, TRequest model, Dictionary<string, string>? parameters = null);
    }
}
