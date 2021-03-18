using ColorBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class SyncService : ISyncService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly ApiHttpClient apiHttpClient;

        private readonly string storageKeyLastUserName = "lastUserName";

        public event Action SyncAvailable;

        public User CurrentUser { get; set; }

        public SyncService(ILocalStorageService localStorageService, ApiHttpClient apiHttpClient)
        {
            this.localStorageService = localStorageService;
            this.apiHttpClient = apiHttpClient;
        }

        public async Task<bool> CheckServerAvailabilityAsync()
        {
            return await apiHttpClient.CheckAvailabilityAsync();
        }

        public async Task<string> GetLastUserNameAsync()
        {
            return await localStorageService.GetItem<string>(storageKeyLastUserName);
        }

        public async Task<bool> LogInAsync(User user, bool stayLoggedId)
        {
            var isValid = await apiHttpClient.ValidateUserAsync(user);

            if (!isValid)
                return false;

            CurrentUser = user;

            await localStorageService.SetItem(storageKeyLastUserName, user.Login);

            if (stayLoggedId)
            {

            }

            SyncAvailable?.Invoke(null, null);

            return true;
        }

        public void LogOut()
        {
            CurrentUser = null;
        }

        public async Task SaveSettingsAsync(Settings settings)
        {
            var isSuccess = await apiHttpClient.SaveSettingsAsync(CurrentUser, settings);
        }

        public async Task<Settings> LoadSettingsAsync()
        {
            return await apiHttpClient.LoadSettingsAsync(CurrentUser);
        }
    }
}
