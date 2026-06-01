using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClientAcquisition.Frontend;
using ClientAcquisition.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient pointing at the backend API. BaseAddress must end with "/api/" so the
// relative endpoints in ApiClient (e.g. "customers") resolve to ".../api/customers".
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "https://client-acquisition-l9g5.onrender.com/api/")
});

builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();