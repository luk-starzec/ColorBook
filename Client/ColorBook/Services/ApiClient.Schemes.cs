using ColorBook.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public partial class ApiClient
    {
        public async Task<bool> SaveLibraryAsync(User user, ColorScheme[] colorSchemes)
        {
            try
            {
                var model = new
                {
                    Schemes = colorSchemes.Select(r => new
                    {
                        r.Id,
                        r.Name,
                        Colors = r.Colors.ToArray(),
                        r.LastUpdate,
                    }).ToArray(),
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/schemes/library")
                    .AddAuthHeaders(user, apiSecret)
                    .AddContent(model);

                var result = await httpClient.SendAsync(request);
                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateSchemeAsync(User user, ColorScheme colorScheme)
        {
            try
            {
                var model = new
                {
                    Scheme = colorScheme
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "/schemes")
                    .AddAuthHeaders(user, apiSecret)
                    .AddContent(model);

                var result = await httpClient.SendAsync(request);
                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteSchemeAsync(User user, Guid colorSchemeId)
        {
            try
            {
                var model = new
                {
                    SchemeId = colorSchemeId
                };

                var request = new HttpRequestMessage(HttpMethod.Delete, "schemes")
                    .AddAuthHeaders(user, apiSecret)
                    .AddContent(model);

                var result = await httpClient.SendAsync(request);
                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ColorScheme[]> LoadLibraryAsync(User user)
        {
            if (user is null)
                return null;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/schemes/library")
                    .AddAuthHeaders(user, apiSecret);

                var result = await httpClient.SendAsync(request);
                result.EnsureSuccessStatusCode();

                var json = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ColorScheme[]>(json, DefaultJsonSerializerOptions.SerializerOptions);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
