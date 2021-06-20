using ColorBook.Models;
using ColorBook.Services.Interfaces;
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
        private readonly ILocalDataStorageService localDataStorageService;
        private readonly ISyncService syncService;


        public SchemeService(ILocalDataStorageService localDataStorageService, ISyncService syncService)
        {
            this.localDataStorageService = localDataStorageService;
            this.syncService = syncService;

            syncService.SyncAvailabilityChanged += async (isAvailable) => await SyncLibrary(isAvailable);
        }

        private async Task SyncLibrary(bool isAvailable)
        {
            if (isAvailable)
                await GetSchemes();
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
            return await localDataStorageService.ExistsColorSchemeAsync(id);
        }

        public async Task<ColorScheme[]> GetSchemes()
        {
            var isSyncAvaliable = await syncService.GetSyncAvailabilityAsync();

            var serverLibrary = isSyncAvaliable ? await syncService.LoadSchemesAsync() : Array.Empty<ColorScheme>();
            serverLibrary = await FilterPreviouslyDeletedSchemes(serverLibrary);
            var localLibrary = await localDataStorageService.GetColorSchemesAsync();

            var schemes = new List<ColorScheme>();
            foreach (var fromLocal in localLibrary)
            {
                var fromServer = serverLibrary.FirstOrDefault(r => r.Id == fromLocal.Id);
                if (fromServer?.LastUpdate > fromLocal.LastUpdate)
                    schemes.Add(fromServer);
                else
                    schemes.Add(fromLocal);
            }

            var onlyServer = serverLibrary.Where(r => !localLibrary.Where(rr => rr.Id == r.Id).Any());
            schemes.AddRange(onlyServer);

            var result = schemes
                .Where(r => r is not null)
                .OrderBy(r => r.Name)
                .ToArray();

            if (isSyncAvaliable)
            {
                foreach (var scheme in result)
                    await UpdateScheme(scheme);

                await syncService.SaveSchemesAsync(result);
            }

            return result;
        }

        private async Task<ColorScheme[]> FilterPreviouslyDeletedSchemes(ColorScheme[] serverLibrary)
        {
            if (!serverLibrary.Any())
                return serverLibrary;

            var deletedSchemes = await localDataStorageService.GetDeletedColorSchemesAsync();

            foreach (var deleted in deletedSchemes)
                await localDataStorageService.RemoveDeletedColorScheme(deleted.Id);

            return serverLibrary
                .Where(r => !deletedSchemes.Where(rr => rr.Id == r.Id && rr.Date > r.LastUpdate).Any())
                .ToArray();
        }

        public async Task RemoveScheme(Guid id)
        {
            await localDataStorageService.RemoveDeletedColorScheme(id);
            await DeleteSchemeOnServer(id);
        }

        private async Task DeleteSchemeOnServer(Guid id)
        {
            var isSyncAvaliable = await syncService.GetSyncAvailabilityAsync();

            if (isSyncAvaliable)
                await syncService.DeleteScheme(id);
            else
                await localDataStorageService.PutDeletedColorSchemeAsync(id);
        }

        public async Task<ColorScheme> AddScheme(ColorScheme scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme.Name))
                scheme.Name = "My Colors";

            scheme.Id = Guid.NewGuid();
            scheme.Name = await DeduplicateName(scheme.Name);

            await UpdateScheme(scheme);

            return scheme;
        }

        private async Task<string> DeduplicateName(string schemeName)
        {
            var result = schemeName;

            var schemesNames = (await GetSchemes()).Select(r => r.Name);

            var marker = schemeName.IndexOf("(");
            var baseName = marker < 0 ? schemeName : schemeName.Substring(0, marker);

            int i = 1;
            while (schemesNames.Where(r => r.ToLower() == result.ToLower()).Any())
                result = $"{baseName}({i++})";

            return result;
        }

        public async Task UpdateScheme(ColorScheme scheme)
        {
            scheme.LastUpdate = DateTime.UtcNow;

            await localDataStorageService.PutColorSchemeAsync(scheme);
            await UpdateSchemeOnServer(scheme);
        }

        private async Task UpdateSchemeOnServer(ColorScheme scheme)
        {
            var isSyncAvaliable = await syncService.GetSyncAvailabilityAsync();

            if (isSyncAvaliable)
                await syncService.UpdateScheme(scheme);
        }

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
