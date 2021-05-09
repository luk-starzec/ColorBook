using ColorBook.Models;
using ColorBook.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class SyncService : ISyncService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly IApiClient apiHttpClient;
        private readonly IJSRuntime js;
        private readonly LocalStorageSettings localStorageSettings;

        private User currentUser = null;
        public event Action<bool> SyncAvailabilityChanged;


        public SyncService(ILocalStorageService localStorageService, IApiClient apiHttpClient, IJSRuntime js, IOptions<LocalStorageSettings> settings)
        {
            this.localStorageService = localStorageService;
            this.apiHttpClient = apiHttpClient;
            this.js = js;
            localStorageSettings = settings.Value;
        }


        private bool isServerAvailable = false;
        private DateTime lastServerAvailabilityCheck = new();
        private readonly int serverAvailabilityCheckInterval = 60;
        public async Task<bool> GetServerAvailabilityAsync()
        {
            if (lastServerAvailabilityCheck.AddSeconds(serverAvailabilityCheckInterval) < DateTime.Now)
            {
                bool previousServerAvailability = isServerAvailable;

                isServerAvailable = await apiHttpClient.CheckServerAvailabilityAsync();
                lastServerAvailabilityCheck = DateTime.Now;

                if (previousServerAvailability != isServerAvailable)
                    SyncAvailabilityChanged?.Invoke(await GetSyncAvailabilityAsync());
            }
            return isServerAvailable;
        }


        public async Task<bool> GetSyncAvailabilityAsync()
        {
            return await GetServerAvailabilityAsync()
                && await GetIsLoggedInAsync();
        }

        public Task<bool> GetIsLoggedInAsync()
        {
            return Task.FromResult(currentUser is not null);
        }

        public async Task<string> GetLastUserNameAsync()
        {
            return await localStorageService.GetItem<string>(localStorageSettings.LastUserLoginKey);
        }

        public User GetLoggedUser() => currentUser;

        public async Task CheckAutoLogIn()
        {
            var passHash = await localStorageService.GetItem<string>(localStorageSettings.LastUserPassKey);
            if (string.IsNullOrEmpty(passHash))
                return;

            var login = await GetLastUserNameAsync();
            if (string.IsNullOrEmpty(login))
                return;

            var password = await js.InvokeAsync<string>("cryptoTools.decryptData", passHash, localStorageSettings.Secret);

            var user = new User(login, password);
            var isValid = await apiHttpClient.ValidateUserAsync(user);
            if (isValid)
            {
                currentUser = user;
                SyncAvailabilityChanged?.Invoke(true);
            }
        }

        public async Task<bool> LogInAsync(User user, bool stayLoggedId)
        {
            var isValid = await apiHttpClient.ValidateUserAsync(user);

            if (!isValid)
                return false;

            currentUser = user;

            await localStorageService.SetItem(localStorageSettings.LastUserLoginKey, user.Login);

            var passHash = stayLoggedId
                ? await js.InvokeAsync<string>("cryptoTools.encryptData", user.Password, localStorageSettings.Secret)
                : null;
            await localStorageService.SetItem(localStorageSettings.LastUserPassKey, passHash);

            SyncAvailabilityChanged?.Invoke(true);

            return true;
        }

        public async Task LogOutAsync()
        {
            currentUser = null;
            await localStorageService.SetItem(localStorageSettings.LastUserPassKey, string.Empty);

            SyncAvailabilityChanged?.Invoke(false);
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
