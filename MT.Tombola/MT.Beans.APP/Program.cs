using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MT.Beans.App;
using MT.Beans.App.Service.BeansApiClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7138/")
});

// Register your API client service
builder.Services.AddScoped<IBeansApiClientService<BeanDto>, BeansApiClientService<BeanDto>>();

await builder.Build().RunAsync();
