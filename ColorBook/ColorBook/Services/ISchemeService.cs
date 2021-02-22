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
        Task<ColorScheme[]> LibraryList();
        Task<bool> ExistsInLibrary(Guid id);
        Task<ColorScheme> LibraryAdd(ColorScheme scheme);
        Task LibrarySave(ColorScheme scheme);
        Task LibraryRemove(Guid id);

        string SchemeToJson(ColorScheme scheme);
        ColorScheme JsonToScheme(string json);
    }
}
