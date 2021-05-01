using ColorBook.Models;
using System;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public interface IApiClient
    {
        Task<bool> CheckServerAvailabilityAsync();
        Task<bool> DeleteSchemeAsync(User user, Guid colorSchemeId);
        Task<ColorScheme[]> LoadLibraryAsync(User user);
        Task<Settings> LoadSettingsAsync(User user);
        Task<bool> SaveLibraryAsync(User user, ColorScheme[] colorSchemes);
        Task<bool> SaveSettingsAsync(User user, Settings settings);
        Task<bool> UpdateSchemeAsync(User user, ColorScheme colorScheme);
        Task<bool> ValidateUserAsync(User user);
    }
}