using ColorBook.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public partial class ApiClient
    {
        public async Task<bool> SaveSettingsAsync(User user, Settings settings)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "/settings")
                    .AddAuthHeaders(user, apiSecret)
                    .AddContent(settings);

                var result = await httpClient.SendAsync(request);
                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Settings> LoadSettingsAsync(User user)
        {
            if (user is null)
                return null;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/settings")
                    .AddAuthHeaders(user, apiSecret);

                var result = await httpClient.SendAsync(request);
                result.EnsureSuccessStatusCode();

                var json = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Settings>(json, DefaultJsonSerializerOptions.SerializerOptions);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
