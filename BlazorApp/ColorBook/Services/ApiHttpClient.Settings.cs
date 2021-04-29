using ColorBook.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public partial class ApiHttpClient
    {
        public async Task<bool> SaveSettingsAsync(User user, Settings settings)
        {
            try
            {
                var model = new
                {
                    user.Login,
                    user.Pass,
                    settings.LightBackgroundColor,
                    settings.DarkBackgroundColor,
                    settings.LightTextColor,
                    settings.DarkTextColor,
                    settings.AutoSync,
                    settings.LastUpdate,
                };
                var data = GetData(model);

                var result = await httpClient.PostAsync("/settings/save", data);
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
                var data = GetData(user);

                var result = await httpClient.PostAsync("/settings/load", data);
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Settings>(json, serializerOptions);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
