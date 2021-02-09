using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    interface ISettingsService
    {
        Task<Settings> GetCurrentSettingsAsync();
        Settings GetDefaultSettings();
        void RestoreDefaultSettings();
        Task SaveSettingsAsync();
        Task SaveSettingsAsync(Settings settings);

        ValueTask SaveTestResult(string result);

    }
}
