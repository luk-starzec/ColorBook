using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public interface ISyncService
    {
        event Action SyncAvailable;
        User CurrentUser { get; set; }

        Task<bool> CheckServerAvailabilityAsync();

        Task<bool> LogInAsync(User user, bool stayLoggedId);
        void LogOut();
        Task<string> GetLastUserNameAsync();

        Task SaveSettingsAsync(Settings settings);
        Task<Settings> LoadSettingsAsync();
    }
}
