﻿using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

namespace ColorBook.Services
{
    public class ApiHttpClient
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return false;
            }
        }

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
                    settings.LastUpdate,
                };
                var data = GetData(model);

                var result = await httpClient.PostAsync("/settings/save", data);
                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return null;
            }
        }

        private StringContent GetData<T>(T model)
        {
            var json = JsonSerializer.Serialize(model, serializerOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }

        private JsonSerializerOptions serializerOptions
            => new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
    }
}
