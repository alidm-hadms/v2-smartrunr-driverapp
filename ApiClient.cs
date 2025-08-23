using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DriverApp
{
    public class ApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        public ApiClient(string baseUrl, string? authToken = null)
        {
            //baseUrl = "https://app.hadmservices.com/api/"; // Default base URL, can be overridden

            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri))
            {
                throw new ArgumentException("Base URL must be a valid absolute URL", nameof(baseUrl));
            }

            _httpClient = new HttpClient { BaseAddress = baseUri };
            if (!string.IsNullOrEmpty(authToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken);
            }


            // _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            // if (!string.IsNullOrEmpty(authToken))
            // {
            //     _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            // }
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            if (result == null)
                throw new InvalidOperationException("Response content could not be deserialized to the specified type.");
            return result;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(request);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<TResponse>(responseContent)!;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}