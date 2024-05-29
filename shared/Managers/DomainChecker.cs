using shared.Interfaces;

namespace shared.Managers
{
    public class DomainChecker : IDomainChecker
    {
        private readonly HttpClient _httpClient;

        public DomainChecker() 
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> IsDomainWorkingAsync(string domain)
        {
            try
            {
                if (!domain.StartsWith("http://") && !domain.StartsWith("https://"))
                {
                    domain = "https://" + domain;
                }

                var response = await _httpClient.GetAsync(domain);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var content = await response.Content.ReadAsStringAsync();

                var metaTagToSearch = """<meta name="domain-verification" content="myelegantpages">""";

                return content.Contains(metaTagToSearch, StringComparison.OrdinalIgnoreCase);
            }
            catch (HttpRequestException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
