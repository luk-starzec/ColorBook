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
        ColorScheme DuplicateScheme(ColorScheme scheme);
        Task<ColorScheme[]> GetShemes();
        Task<bool> ExistsInLibrary(Guid id);
        Task<ColorScheme> AddScheme(ColorScheme scheme);
        Task UpdateScheme(ColorScheme scheme);
        Task RemoveScheme(Guid id);

        string SchemeToJson(ColorScheme scheme);
        ColorScheme JsonToScheme(string json);
    }
}
