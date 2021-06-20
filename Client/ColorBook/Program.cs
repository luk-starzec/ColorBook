using ColorBook.Models;
using ColorBook.Services;
using ColorBook.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ColorBook
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var configuration = builder.Configuration.Build();
            var apiUrl = configuration.GetValue<string>("ServerApiUrl");
            var apiSecret = configuration.GetValue<string>("ServerApiSecret");
            var localStorageSettings = configuration.GetSection("LocalStorageSettings");

            builder.Services.Configure<LocalStorageSettings>(options => localStorageSettings.Bind(options));

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddHttpClient("ApiClient", client => client.BaseAddress = new Uri(apiUrl));

            builder.Services.AddScoped<IApiClient>(ctx =>
            {
                var clientFactory = ctx.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient("ApiClient");

                return new ApiClient(httpClient, apiSecret);
            });

            builder.Services.AddScoped<ISettingsService, SettingsService>();
            builder.Services.AddScoped<ISchemeService, SchemeService>();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddScoped<ILocalDataStorageService, LocalDataStorageService>();
            builder.Services.AddScoped<ISyncService, SyncService>();

            builder.Logging.SetMinimumLevel(LogLevel.Warning);

            await builder.Build().RunAsync();
        }
    }
}
