using ColorBook.Models;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ColorBook.Services
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage AddAuthHeaders(this HttpRequestMessage request, User user, string apiSecret)
        {
            request.Headers.Add("x-user", user.Login);
            request.Headers.Add("x-access", GetAccessHeader(user.Password, apiSecret));

            return request;
        }

        public static HttpRequestMessage AddContent<T>(this HttpRequestMessage request, T model)
        {
            request.Content = GetData(model);
            return request;
        }

        private static string GetAccessHeader(string pass, string apiSecret)
        {
            var passHash = GetHash(pass);
            var hash = GetHash($"{apiSecret}{passHash}");

            return hash;
        }

        private static string GetHash(string input)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
            string hash = Convert.ToBase64String(hashBytes);

            return hash;
        }

        private static StringContent GetData<T>(T model)
        {
            var json = JsonSerializer.Serialize(model, DefaultJsonSerializerOptions.SerializerOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }
    }

    public static class DefaultJsonSerializerOptions
    {
        public static JsonSerializerOptions SerializerOptions
            => new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
    }
}
