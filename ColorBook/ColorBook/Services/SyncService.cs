using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class SyncService : ISyncService
    {
        private readonly ILocalStorageService localStorageService;

        public SyncService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

    }
}
