using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public interface ISyncService
    {
        event Action<bool> SyncAvailabilityChanged;
        Task<bool> GetSyncAvailabilityAsync();

        Task<bool> GetServerAvailabilityAsync();
        Task<bool> GetIsLoggedInAsync();

        User GetLoggedUser();

        Task<bool> LogInAsync(User user, bool stayLoggedId);
        Task LogOutAsync();
        Task<string> GetLastUserNameAsync();

        Task SaveSettingsAsync(Settings settings);
        Task<Settings> LoadSettingsAsync();

        Task SaveSchemesAsync(ColorScheme[] colorSchemes);
        Task<ColorScheme[]> LoadSchemesAsync();
    }
}
