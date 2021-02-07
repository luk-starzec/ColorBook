using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly Settings defaultSettings;
        private Settings currentSettings;

        public SettingsService()
        {
            defaultSettings = new Settings
            {
                LightBackgroundColor = "#FFFFFF",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#FFFFFF",
                DarkTextColor = "#333333"
            };

            currentSettings = new Settings
            {
                LightBackgroundColor = "#FFFFFF",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#FFFFFF",
                DarkTextColor = "#333333"
            };
        }

        public Settings GetCurrentSettings() => currentSettings;

        public Settings GetDefaultSettings() => defaultSettings;

        public void RestoreDefaultSettings()
        {
            currentSettings.LightBackgroundColor = defaultSettings.LightBackgroundColor;
            currentSettings.DarkBackgroundColor = defaultSettings.DarkBackgroundColor;
            currentSettings.LightTextColor = defaultSettings.LightTextColor;
            currentSettings.DarkTextColor = defaultSettings.DarkTextColor;
        }
    }
}
