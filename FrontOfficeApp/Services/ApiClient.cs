using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FrontOfficeApp.Services;

public interface IApiClient
{
    void SetToken(string token);
    Task<T?> GetAsync<T>(string path);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest payload);
}

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7001/api/")
        };
    }

    public void SetToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public Task<T?> GetAsync<T>(string path)
        => _httpClient.GetFromJsonAsync<T>(path);

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest payload)
    {
        var response = await _httpClient.PostAsJsonAsync(path, payload);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}
