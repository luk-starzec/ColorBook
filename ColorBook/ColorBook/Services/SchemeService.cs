using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class SchemeService : ISchemeService
    {
        public ColorScheme GetEmptyScheme()
        {
            return new ColorScheme
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Colors = new List<NamedColor>
                {
                    new NamedColor("#333333"),
                },
            };
        }

        public Task<bool> ExistsInLibraryAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ColorScheme[]> LibraryListAsync()
        {
            var list = new ColorScheme[]
            {
                new ColorScheme
                {
                    Id = Guid.NewGuid(),
                    Name = "Test 001",
                    Colors = new List<NamedColor>
                    {
                        new NamedColor("#0000ff"),
                    },
                },
                new ColorScheme
                {
                    Id = Guid.NewGuid(),
                    Name = "Test 002",
                    Colors = new List<NamedColor>
                    {
                        new NamedColor("#4DEA26"),
                        new NamedColor("#DDEEFF"),
                    },
                },
            };
            return await Task.FromResult(list);
        }

        public Task LibraryRemoveAsync(ColorScheme scheme)
        {
            throw new NotImplementedException();
        }

        public Task LibrarySaveAsync(ColorScheme scheme)
        {
            throw new NotImplementedException();
        }
    }
}
