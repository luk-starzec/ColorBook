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
        private readonly Settings defaultSettings;
        private Settings currentSettings = null;

        public SettingsService(IJSRuntime js)
        {
            this.js = js;

            defaultSettings = new Settings
            {
                LightBackgroundColor = "#FFFFFF",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#FFFFFF",
                DarkTextColor = "#333333"
            };
        }

        public async Task<Settings> GetCurrentSettingsAsync()
        {
            if (currentSettings == null)
            {
                currentSettings = await js.InvokeAsync<Settings>($"localDataStore.get", "settings", "current");

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
        }

    }
}
