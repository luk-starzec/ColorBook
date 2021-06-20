using ColorBook.Models;
using ColorBook.Services.Interfaces;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class LocalDataStorageService : ILocalDataStorageService
    {
        private readonly IJSRuntime js;

        private readonly string schemesDataStoreName = "schemes";
        private readonly string deletedSchemesDataStoreName = "deletedSchemes";
        private readonly string settingsDataStoreName = "settings";

        public LocalDataStorageService(IJSRuntime js)
        {
            this.js = js;
        }

        public async Task<bool> ExistsColorSchemeAsync(Guid id)
        {
            return await js.InvokeAsync<bool>($"localDataStore.exists", schemesDataStoreName, id.ToString());
        }

        public async Task<ColorScheme[]> GetColorSchemesAsync()
        {
            return await js.InvokeAsync<ColorScheme[]>($"localDataStore.getAll", schemesDataStoreName);
        }

        public async Task PutColorSchemeAsync(ColorScheme colorScheme)
        {
            await js.InvokeVoidAsync($"localDataStore.put", schemesDataStoreName, colorScheme.Id.ToString(), colorScheme);
        }

        public async Task<DeletedColorScheme[]> GetDeletedColorSchemesAsync()
        {
            return await js.InvokeAsync<DeletedColorScheme[]>($"localDataStore.getAll", deletedSchemesDataStoreName);
        }

        public async Task PutDeletedColorSchemeAsync(Guid id)
        {
            await js.InvokeVoidAsync($"localDataStore.put", deletedSchemesDataStoreName, id.ToString(), new DeletedColorScheme(id, DateTime.Now));
        }

        public async Task RemoveDeletedColorScheme(Guid colorSchemeId)
        {
            await js.InvokeVoidAsync($"localDataStore.delete", deletedSchemesDataStoreName, colorSchemeId);
        }

        public async Task<Settings> GetSettingsAsync()
        {
            return await js.InvokeAsync<Settings>($"localDataStore.get", settingsDataStoreName, "current");
        }

        public async Task PutSettingsAsync(Settings settings)
        {
            await js.InvokeVoidAsync($"localDataStore.put", settingsDataStoreName, "current", settings);
        }
    }
}
