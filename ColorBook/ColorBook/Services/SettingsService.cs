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
            syncService.SyncAvailabilityChanged += async (isAvailable) => await SyncSettings(isAvailable);

            defaultSettings = new Settings
            {
                LightBackgroundColor = "#FFFFFF",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#FFFFFF",
                DarkTextColor = "#333333"
            };
        }

        public event Action<Settings> CurrentSettingsChanged;

        public async Task<Settings> GetCurrentSettingsAsync()
        {
            if (currentSettings is not null)
                return currentSettings;

            var serverSettings = await syncService.LoadSettingsAsync();
            var localSettings = await js.InvokeAsync<Settings>($"localDataStore.get", "settings", "current");

            currentSettings = GetNewerSettings(serverSettings, localSettings);

            if (currentSettings == null)
                currentSettings = new Settings
                {
                    LightBackgroundColor = defaultSettings.LightBackgroundColor,
                    DarkBackgroundColor = defaultSettings.DarkBackgroundColor,
                    LightTextColor = defaultSettings.LightTextColor,
                    DarkTextColor = defaultSettings.DarkTextColor,
                };
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
            currentSettings.AutoSync = settings.AutoSync;
            currentSettings.LastUpdate = DateTime.Now;

            await js.InvokeVoidAsync($"localDataStore.put", "settings", "current", currentSettings);

            if (currentSettings.AutoSync)
                await syncService.SaveSettingsAsync(currentSettings);

            CurrentSettingsChanged?.Invoke(currentSettings);
        }

        public async Task PullSettingsFromServer()
        {
            var serverSettings = await syncService.LoadSettingsAsync();
            if (serverSettings == null)
                return;

            await SaveSettingsAsync(serverSettings);
        }

        public async Task PushSettingsToServer()
        {
            await syncService.SaveSettingsAsync(currentSettings);
        }

        private async Task SyncSettings(bool isAvailable)
        {
            if (!isAvailable)
                return;
            if (currentSettings?.AutoSync != true)
                return;

            var serverSettings = await syncService.LoadSettingsAsync();

            currentSettings = GetNewerSettings(serverSettings, currentSettings);

            if (currentSettings != serverSettings)
                await syncService.SaveSettingsAsync(currentSettings);

            CurrentSettingsChanged?.Invoke(currentSettings);
        }

        private Settings GetNewerSettings(Settings primary, Settings secondary)
        {
            if (secondary?.LastUpdate is null && primary is not null)
                return primary;

            if (primary?.LastUpdate is null && secondary is not null)
                return secondary;

            return primary?.LastUpdate > secondary?.LastUpdate ? primary : secondary;
        }
    }
}
