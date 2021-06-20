using ColorBook.Models;
using System;
using System.Threading.Tasks;

namespace ColorBook.Services.Interfaces
{
    public interface ISyncService
    {
        event Action<bool> SyncAvailabilityChanged;
        Task<bool> GetSyncAvailabilityAsync();

        Task<bool> GetServerAvailabilityAsync();
        Task<bool> GetIsLoggedInAsync();

        User GetLoggedUser();

        Task CheckAutoLogIn();
        Task<bool> LogInAsync(User user, bool stayLoggedId);
        Task LogOutAsync();
        Task<string> GetLastUserNameAsync();

        Task SaveSettingsAsync(Settings settings);
        Task<Settings> LoadSettingsAsync();

        Task SaveSchemesAsync(ColorScheme[] colorSchemes);
        Task<ColorScheme[]> LoadSchemesAsync();
        Task DeleteScheme(Guid id);
        Task UpdateScheme(ColorScheme colorScheme);
    }
}
