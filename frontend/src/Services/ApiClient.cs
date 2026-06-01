using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClientAcquisition.Frontend.Models;
using ClientAcquisition.Frontend.Models.Forms;

namespace ClientAcquisition.Frontend.Services;

public sealed class ApiClient
{
    private const int MaxPageSize = 1000;
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Customers
    public async Task<List<CustomerResponseDto>> GetCustomersAsync(int page = 1, int pageSize = MaxPageSize)
    {
        var response = await _httpClient.GetAsync($"customers?page={page}&pageSize={pageSize}");
        return await ReadAsync<List<CustomerResponseDto>>(response) ?? new List<CustomerResponseDto>();
    }

    public async Task<CustomerResponseDto?> GetCustomerAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"customers/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return await ReadAsync<CustomerResponseDto>(response);
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(CustomerForm form)
    {
        var response = await _httpClient.PostAsJsonAsync("customers", form.ToPayload());
        return (await ReadAsync<CustomerResponseDto>(response))!;
    }

    public async Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, CustomerForm form)
    {
        var response = await _httpClient.PutAsJsonAsync($"customers/{id}", form.ToPayload());
        return (await ReadAsync<CustomerResponseDto>(response))!;
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"customers/{id}");
        await EnsureSuccessAsync(response);
    }

    // Orders
    public async Task<List<OrderResponseDto>> GetOrdersAsync(string? customerName = null, DateTime? startDate = null, DateTime? endDate = null, int page = 1, int pageSize = MaxPageSize)
    {
        var query = new List<string> { $"page={page}", $"pageSize={pageSize}" };
        if (!string.IsNullOrWhiteSpace(customerName))
        {
            query.Add($"customerName={Uri.EscapeDataString(customerName)}");
        }

        if (startDate is not null)
        {
            query.Add($"startDate={Uri.EscapeDataString(startDate.Value.ToString("o"))}");
        }

        if (endDate is not null)
        {
            query.Add($"endDate={Uri.EscapeDataString(endDate.Value.ToString("o"))}");
        }

        var response = await _httpClient.GetAsync($"orders?{string.Join("&", query)}");
        return await ReadAsync<List<OrderResponseDto>>(response) ?? new List<OrderResponseDto>();
    }

    public async Task<OrderResponseDto?> GetOrderAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"orders/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return await ReadAsync<OrderResponseDto>(response);
    }

    public async Task<OrderResponseDto> CreateOrderAsync(OrderForm form)
    {
        var response = await _httpClient.PostAsJsonAsync("orders", form.ToPayload());
        return (await ReadAsync<OrderResponseDto>(response))!;
    }

    public async Task<OrderResponseDto> UpdateOrderAsync(Guid id, OrderForm form)
    {
        var payload = new
        {
            orderDate = form.OrderDate,
            items = form.Items.Select(item => new
            {
                productName = item.ProductName,
                quantity = item.Quantity,
                unitPrice = item.UnitPrice
            })
        };

        var response = await _httpClient.PutAsJsonAsync($"orders/{id}", payload);
        return (await ReadAsync<OrderResponseDto>(response))!;
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"orders/{id}");
        await EnsureSuccessAsync(response);
    }

    // Reports
    public async Task<List<CustomerSpendingDto>> GetCustomerSpendingAsync()
    {
        var response = await _httpClient.GetAsync("reports/customer-spending");
        return await ReadAsync<List<CustomerSpendingDto>>(response) ?? new List<CustomerSpendingDto>();
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private async Task<T?> ReadAsync<T>(HttpResponseMessage response)
    {
        await EnsureSuccessAsync(response);
        if (response.Content.Headers.ContentLength == 0)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    private async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var message = await ExtractErrorMessageAsync(response);
        throw new ApiException(message);
    }

    private static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
    {
        try
        {
            var body = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(body))
            {
                using var document = JsonDocument.Parse(body);
                if (document.RootElement.TryGetProperty("detail", out var detail) && detail.ValueKind == JsonValueKind.String)
                {
                    return detail.GetString()!;
                }

                if (document.RootElement.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String)
                {
                    return title.GetString()!;
                }
            }
        }
        catch
        {
            // fall through to the generic message below
        }

        return response.StatusCode switch
        {
            HttpStatusCode.NotFound => "The requested resource was not found.",
            HttpStatusCode.BadRequest => "The request could not be processed.",
            _ => "Could not reach the server. Please try again."
        };
    }
}
