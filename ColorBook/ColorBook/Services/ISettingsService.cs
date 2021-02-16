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
        Task RestoreDefaultSettings();
        Task SaveSettingsAsync();
        Task SaveSettingsAsync(Settings settings);

    }
}
