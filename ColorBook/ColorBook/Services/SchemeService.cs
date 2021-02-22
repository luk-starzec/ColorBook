using ColorBook.Models;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class SchemeService : ISchemeService
    {
        private readonly IJSRuntime js;

        public SchemeService(IJSRuntime js)
        {
            this.js = js;
        }

        public ColorScheme GetEmptyScheme()
        {
            return new ColorScheme
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Colors = new List<NamedColor>
                {
                    new NamedColor("#333333"),
                },
                LastUpdate = DateTime.Now,
            };
        }

        public async Task<bool> ExistsInLibrary(Guid id)
        {
            return await js.InvokeAsync<bool>($"localDataStore.exists", "library", id.ToString());
        }

        public async Task<ColorScheme[]> LibraryList()
        {
            return await js.InvokeAsync<ColorScheme[]>($"localDataStore.getAll", "library");
        }

        public async Task LibraryRemove(Guid id)
        {
            await js.InvokeAsync<ColorScheme[]>($"localDataStore.delete", "library", id);
        }

        public async Task<ColorScheme> LibraryAdd(ColorScheme scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme.Name))
                scheme.Name = "My Colors";

            scheme.Id = Guid.NewGuid();
            scheme.Name = await DeduplicateName(scheme.Name);

            await LibrarySave(scheme);

            return scheme;
        }

        private async Task<string> DeduplicateName(string schemeName)
        {
            var result = schemeName;

            var schemesNames = (await LibraryList()).Select(r => r.Name);

            var marker = schemeName.IndexOf("(");
            var baseName = marker < 0 ? schemeName : schemeName.Substring(0, marker);

            int i = 1;
            while (schemesNames.Where(r => r.ToLower() == result.ToLower()).Any())
                result = $"{baseName}({i++})";

            return result;
        }

        public async Task LibrarySave(ColorScheme scheme)
        {
            scheme.LastUpdate = DateTime.Now;
            await js.InvokeVoidAsync($"localDataStore.put", "library", scheme.Id.ToString(), scheme);
        }

        ValueTask PutAsync<T>(string storeName, object key, T value)
            => js.InvokeVoidAsync($"localDataStore.put", storeName, key, value);

        public string SchemeToJson(ColorScheme scheme)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return JsonSerializer.Serialize(scheme, options);
        }

        public ColorScheme JsonToScheme(string json)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return JsonSerializer.Deserialize<ColorScheme>(json, options);
        }

        public ColorScheme DuplicateScheme(ColorScheme scheme)
        {
            return new ColorScheme
            {
                Id = Guid.NewGuid(),
                Name = string.IsNullOrWhiteSpace(scheme.Name) ? string.Empty : $"{scheme.Name} (copy)",
                Colors = scheme.Colors.ToArray(),
                LastUpdate = DateTime.Now,
            };
        }
    }
}
