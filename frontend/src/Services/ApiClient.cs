using System.Net.Http.Json;
using ClientAcquisition.Frontend.Models;

namespace ClientAcquisition.Frontend.Services;

public sealed class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CustomerResponseDto>> GetCustomersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CustomerResponseDto>>("customers") ?? new List<CustomerResponseDto>();
    }

    public async Task<List<OrderResponseDto>> GetOrdersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<OrderResponseDto>>("orders") ?? new List<OrderResponseDto>();
    }
}