using ColorBook.Services;
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
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddHttpClient<ApiHttpClient>(client => client.BaseAddress = new Uri(apiUrl));

            builder.Services.AddScoped<ISettingsService, SettingsService>();
            builder.Services.AddScoped<ISchemeService, SchemeService>();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddScoped<ISyncService, SyncService>();

            builder.Logging.SetMinimumLevel(LogLevel.Warning);

            await builder.Build().RunAsync();
        }
    }
}
