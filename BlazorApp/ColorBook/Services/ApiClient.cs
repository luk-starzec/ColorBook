using ColorBook.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public partial class ApiClient
    {
        private readonly HttpClient httpClient;
        private readonly string apiSecret;

        public ApiClient(HttpClient httpClient, string apiSecret)
        {
            this.httpClient = httpClient;
            this.apiSecret = apiSecret;
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
                var request = new HttpRequestMessage(HttpMethod.Get, "/users/validate")
                    .AddAuthHeaders(user, apiSecret);

                var result = await httpClient.SendAsync(request);
                return result.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
