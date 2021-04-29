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

        private User currentUser = null;

        public event Action<bool> SyncAvailabilityChanged;


        public SyncService(ILocalStorageService localStorageService, ApiHttpClient apiHttpClient)
        {
            this.localStorageService = localStorageService;
            this.apiHttpClient = apiHttpClient;
        }


        private bool isServerAvailable = false;
        private DateTime lastServerAvailabilityCheck = new DateTime();
        private int serverAvailabilityCheckInterval = 10;
        public async Task<bool> GetServerAvailabilityAsync()
        {
            if (lastServerAvailabilityCheck.AddSeconds(serverAvailabilityCheckInterval) < DateTime.Now)
            {
                isServerAvailable = await apiHttpClient.CheckServerAvailabilityAsync();
                lastServerAvailabilityCheck = DateTime.Now;

                SyncAvailabilityChanged?.Invoke(await GetSyncAvailabilityAsync());
            }
            return isServerAvailable;
        }


        public async Task<bool> GetSyncAvailabilityAsync()
        {
            return await GetServerAvailabilityAsync()
                && await GetIsLoggedInAsync();
        }

        public Task<bool> GetIsLoggedInAsync() => Task.FromResult(currentUser is not null);

        public async Task<string> GetLastUserNameAsync()
            => await localStorageService.GetItem<string>(storageKeyLastUserName);

        public User GetLoggedUser() => currentUser;

        public async Task<bool> LogInAsync(User user, bool stayLoggedId)
        {
            var isValid = await apiHttpClient.ValidateUserAsync(user);

            if (!isValid)
                return false;

            currentUser = user;

            await localStorageService.SetItem(storageKeyLastUserName, user.Login);

            if (stayLoggedId)
            {

            }

            SyncAvailabilityChanged?.Invoke(true);

            return true;
        }

        public Task LogOutAsync()
        {
            currentUser = null;
            SyncAvailabilityChanged?.Invoke(false);
            return Task.CompletedTask;
        }


        public async Task SaveSettingsAsync(Settings settings)
        {
            var isSuccess = await apiHttpClient.SaveSettingsAsync(currentUser, settings);
        }

        public async Task<Settings> LoadSettingsAsync()
        {
            return await apiHttpClient.LoadSettingsAsync(currentUser);
        }

        public async Task SaveSchemesAsync(ColorScheme[] colorSchemes)
        {
            var isSuccess = await apiHttpClient.SaveLibraryAsync(currentUser, colorSchemes);
        }

        public async Task<ColorScheme[]> LoadSchemesAsync()
        {
            return await apiHttpClient.LoadLibraryAsync(currentUser);
        }

        public async Task DeleteScheme(Guid id)
        {
            var isSuccess = await apiHttpClient.DeleteSchemeAsync(currentUser, id);
        }

        public async Task UpdateScheme(ColorScheme colorScheme)
        {
            var isSuccess = await apiHttpClient.UpdateSchemeAsync(currentUser, colorScheme);
        }
    }
}
