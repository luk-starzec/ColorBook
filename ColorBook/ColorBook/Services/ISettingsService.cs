using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    interface ISettingsService
    {
        Settings GetCurrentSettings();
        Settings GetDefaultSettings();
        void RestoreDefaultSettings();
    }
}
