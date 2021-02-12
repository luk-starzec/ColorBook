using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    interface ISchemeService
    {
        ColorScheme GetEmptyScheme();
        Task<ColorScheme[]> LibraryListAsync();
        Task<bool> ExistsInLibraryAsync(Guid id);
        Task LibrarySaveAsync(ColorScheme scheme);
        Task LibraryRemoveAsync(ColorScheme scheme);

    }
}
