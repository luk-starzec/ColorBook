using ColorBook.Models;
using System;
using System.Threading.Tasks;

namespace ColorBook.Services.Interfaces
{
    public interface ISchemeService
    {
        ColorScheme GetEmptyScheme();
        ColorScheme DuplicateScheme(ColorScheme scheme);
        Task<ColorScheme[]> GetSchemes();
        Task<bool> ExistsInLibrary(Guid id);
        Task<ColorScheme> AddScheme(ColorScheme scheme);
        Task UpdateScheme(ColorScheme scheme);
        Task RemoveScheme(Guid id);

        string SchemeToJson(ColorScheme scheme);
        ColorScheme JsonToScheme(string json);
    }
}
