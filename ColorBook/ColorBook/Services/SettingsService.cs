using ColorBook.Models;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IJSRuntime js;
        private readonly ISyncService syncService;
        private readonly Settings defaultSettings;
        private Settings currentSettings = null;

        public SettingsService(IJSRuntime js, ISyncService syncService)
        {
            this.js = js;
            this.syncService = syncService;
            syncService.SyncAvailable += async () => await SyncSettings();

            defaultSettings = new Settings
            {
                LightBackgroundColor = "#FFFFFF",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#FFFFFF",
                DarkTextColor = "#333333"
            };
        }

        private async Task SyncSettings()
        {
            var serverSettings = await syncService.LoadSettingsAsync();
            if (serverSettings is not null)
                currentSettings = serverSettings;
        }

        public async Task<Settings> GetCurrentSettingsAsync()
        {
            if (currentSettings == null)
            {
                var serverSettings = await syncService.LoadSettingsAsync();
                var localSettings = await js.InvokeAsync<Settings>($"localDataStore.get", "settings", "current");

                // TODO decyzja wg daty modyfikacji
                if (serverSettings is not null)
                    currentSettings = serverSettings;
                else
                    currentSettings = localSettings;

                if (currentSettings == null)
                    currentSettings = new Settings
                    {
                        LightBackgroundColor = defaultSettings.LightBackgroundColor,
                        DarkBackgroundColor = defaultSettings.DarkBackgroundColor,
                        LightTextColor = defaultSettings.LightTextColor,
                        DarkTextColor = defaultSettings.DarkTextColor,
                    };
            }
            return currentSettings;
        }

        public Settings GetDefaultSettings() => defaultSettings;

        public async Task RestoreDefaultSettings() => await SaveSettingsAsync(defaultSettings);

        public async Task SaveSettingsAsync() => await SaveSettingsAsync(currentSettings);

        public async Task SaveSettingsAsync(Settings settings)
        {
            currentSettings.LightBackgroundColor = settings.LightBackgroundColor;
            currentSettings.DarkBackgroundColor = settings.DarkBackgroundColor;
            currentSettings.LightTextColor = settings.LightTextColor;
            currentSettings.DarkTextColor = settings.DarkTextColor;

            await js.InvokeVoidAsync($"localDataStore.put", "settings", "current", settings);
            await syncService.SaveSettingsAsync(settings);
        }

    }
}
