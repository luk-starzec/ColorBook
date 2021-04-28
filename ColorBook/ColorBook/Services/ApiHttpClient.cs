using ColorBook.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public partial class ApiHttpClient
    {
        private readonly HttpClient httpClient;

        public ApiHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> CheckServerAvailabilityAsync()
        {
            try
            {
                var result = await httpClient.GetAsync("/hc");
                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ValidateUserAsync(User user)
        {
            try
            {
                var data = GetData(user);

                var result = await httpClient.PostAsync("/users/validate", data);
                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private StringContent GetData<T>(T model)
        {
            var json = JsonSerializer.Serialize(model, serializerOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }

        private JsonSerializerOptions serializerOptions
            => new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
    }
}
