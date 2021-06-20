using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services.Interfaces
{
    public interface ILocalDataStorageService
    {
        Task<ColorScheme[]> GetColorSchemesAsync();
        Task PutColorSchemeAsync(ColorScheme colorScheme);
        Task RemoveDeletedColorScheme(Guid id);
        Task<DeletedColorScheme[]> GetDeletedColorSchemesAsync();
        Task PutDeletedColorSchemeAsync(Guid id);
        Task<bool> ExistsColorSchemeAsync(Guid id);

        Task<Settings> GetSettingsAsync();
        Task PutSettingsAsync(Settings settings);
    }
}
