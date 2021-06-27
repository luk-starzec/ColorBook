using ColorBook.Models;
using ColorBook.Services;
using ColorBook.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ColorBookTests.Services
{
    public class SyncServiceTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetServerAvailabilityAsync_ReturnsServerAvailabitity(bool isAvailable)
        {
            var apiHttpClientStub = new Mock<IApiClient>();
            apiHttpClientStub.Setup(r => r.CheckServerAvailabilityAsync())
                .Returns(Task.FromResult(isAvailable));

            var sut = GetSyncService(apiHttpClientStub);

            var actual = await sut.GetServerAvailabilityAsync();

            Assert.Equal(isAvailable, actual);
        }

        [Fact]
        public async Task LogInAsync_WhenValidUser_ReturnsTrue()
        {
            var user = new User("test", "");

            var apiHttpClientStub = new Mock<IApiClient>();
            apiHttpClientStub.Setup(r => r.ValidateUserAsync(user))
                .Returns(Task.FromResult(true));

            var sut = GetSyncService(apiHttpClientStub);

            var actual = await sut.LogInAsync(user, false);

            Assert.True(actual);
        }

        [Fact]
        public async Task LogInAsync_WhenInvalidUser_ReturnsFalse()
        {
            var user = new User("test", "");

            var apiHttpClientStub = new Mock<IApiClient>();
            apiHttpClientStub.Setup(r => r.ValidateUserAsync(user))
                .Returns(Task.FromResult(false));

            var sut = GetSyncService(apiHttpClientStub);

            var actual = await sut.LogInAsync(user, false);

            Assert.False(actual);
        }

        [Fact]
        public async Task LogInAsync_WhenValidUser_SetsLoggedUser()
        {
            var user = new User("test", "");

            var apiHttpClientStub = new Mock<IApiClient>();
            apiHttpClientStub.Setup(r => r.ValidateUserAsync(user))
                .Returns(Task.FromResult(true));

            var sut = GetSyncService(apiHttpClientStub);

            await sut.LogInAsync(user, false);
            var actual = sut.GetLoggedUser();

            Assert.Equal(user, actual);
        }

        [Fact]
        public async Task LogInAsync_WhenValidUserAndStayLoggedIn_SetsUserAndPassInLocalStorage()
        {
            var user = new User("test", "");

            var localStorageServiceStub = new Mock<ILocalStorageService>();

            var apiHttpClientStub = new Mock<IApiClient>();
            apiHttpClientStub.Setup(r => r.ValidateUserAsync(user))
                .Returns(Task.FromResult(true));

            var sut = GetSyncService(apiHttpClientStub, localStorageServiceStub);

            await sut.LogInAsync(user, true);

            localStorageServiceStub.Verify(r => r.SetItem(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task LogOutAsync_ClearsLoggedUser()
        {
            var user = new User("test", "");

            var apiHttpClientStub = new Mock<IApiClient>();
            apiHttpClientStub.Setup(r => r.ValidateUserAsync(user))
                .Returns(Task.FromResult(true));

            var sut = GetSyncService(apiHttpClientStub);

            await sut.LogInAsync(user, false);
            await sut.LogOutAsync();
            var actual = sut.GetLoggedUser();

            Assert.Null(actual);
        }

        [Fact]
        public async Task LogOutAsync_ClearsUserInLocalStorage()
        {
            var user = new User("test", "");

            var localStorageServiceStub = new Mock<ILocalStorageService>();

            var apiHttpClientStub = new Mock<IApiClient>();
            apiHttpClientStub.Setup(r => r.ValidateUserAsync(user))
                .Returns(Task.FromResult(true));

            var sut = GetSyncService(apiHttpClientStub, localStorageServiceStub);

            await sut.LogOutAsync();

            localStorageServiceStub.Verify(r => r.SetItem(It.IsAny<string>(), string.Empty), Times.Once);
        }


        private SyncService GetSyncService(Mock<IApiClient> apiHttpClientStub = null, Mock<ILocalStorageService> localStorageServiceStub = null)
        {
            if (localStorageServiceStub is null)
                localStorageServiceStub = new Mock<ILocalStorageService>();

            if (apiHttpClientStub is null)
                apiHttpClientStub = new Mock<IApiClient>();

            var jsRuntimeStub = new Mock<IJSRuntime>();

            var options = new Mock<IOptions<LocalStorageSettings>>();
            options.Setup(r => r.Value).Returns(new LocalStorageSettings());

            return new SyncService(localStorageServiceStub.Object, apiHttpClientStub.Object, jsRuntimeStub.Object, options.Object);
        }
    }
}
