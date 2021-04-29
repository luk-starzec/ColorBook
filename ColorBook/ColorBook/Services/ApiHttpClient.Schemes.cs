using ColorBook.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public partial class ApiHttpClient
    {
        public async Task<bool> SaveLibraryAsync(User user, ColorScheme[] colorSchemes)
        {
            try
            {
                var model = new
                {
                    user.Login,
                    user.Pass,
                    Schemes = colorSchemes.Select(r => new
                    {
                        r.Id,
                        r.Name,
                        Colors = r.Colors.ToArray(),
                        r.LastUpdate,
                    }).ToArray(),
                };
                var data = GetData(model);

                var result = await httpClient.PostAsync("/schemes/saveLibrary", data);
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
                    user.Login,
                    user.Pass,
                    Scheme = colorScheme
                };
                var data = GetData(model);

                var result = await httpClient.PostAsync("/schemes/save", data);
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
                    user.Login,
                    user.Pass,
                    SchemeId = colorSchemeId
                };
                var data = GetData(model);

                var result = await httpClient.PostAsync("/schemes/delete", data);
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
                var data = GetData(user);

                var result = await httpClient.PostAsync("/schemes/loadLibrary", data);
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ColorScheme[]>(json, serializerOptions);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
