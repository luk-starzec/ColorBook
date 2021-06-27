using ColorBook.Models;
using ColorBook.Services;
using ColorBook.Services.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ColorBookTests.Services
{
    public class SettingsServiceTests
    {
        [Fact]
        public async Task GetCurrentSettingsAsync_WhenOnlyServerSettings_ReturnsServerSettings()
        {
            var fromServer = new Settings
            {
                LightBackgroundColor = "#ffffff",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#eeeeee",
                DarkTextColor = "#111111",
            };
            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.LoadSettingsAsync())
                .Returns(Task.FromResult(fromServer));
            var sut = GetSettingsService(syncServiceStub: syncServiceStub);

            var actual = await sut.GetCurrentSettingsAsync();

            Assert.True(IsEqual(fromServer, actual));
        }

        [Fact]
        public async Task GetCurrentSettingsAsync_WhenOnlyLocalSettings_ReturnsLocalSettings()
        {
            var fromLocal = new Settings
            {
                LightBackgroundColor = "#ffffff",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#eeeeee",
                DarkTextColor = "#111111",
            };
            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            localDataStorageServiceStub.Setup(r => r.GetSettingsAsync())
                .Returns(Task.FromResult(fromLocal));
            var sut = GetSettingsService(localDataStorageServiceStub);

            var actual = await sut.GetCurrentSettingsAsync();

            Assert.True(IsEqual(fromLocal, actual));
        }

        [Fact]
        public async Task GetCurrentSettingsAsync_WhenServerAndLocalSettings_ReturnsNewerSettings()
        {
            var fromServer = new Settings
            {
                LightBackgroundColor = "#fffff0",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#eeeee0",
                DarkTextColor = "#111110",
                LastUpdate = DateTime.Now,
            };
            var fromLocal = new Settings
            {
                LightBackgroundColor = "#fffff1",
                DarkBackgroundColor = "#000001",
                LightTextColor = "#eeeee1",
                DarkTextColor = "#111111",
                LastUpdate = DateTime.Now.AddDays(-1),
            };

            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            localDataStorageServiceStub.Setup(r => r.GetSettingsAsync())
                .Returns(Task.FromResult(fromLocal));

            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.LoadSettingsAsync())
                .Returns(Task.FromResult(fromServer));

            var sut = GetSettingsService(localDataStorageServiceStub, syncServiceStub);

            var actual = await sut.GetCurrentSettingsAsync();

            Assert.True(IsEqual(fromServer, actual));
        }

        [Fact]
        public async Task GetCurrentSettingsAsync_WhenNoSettings_ReturnsDefaultsSettings()
        {
            var sut = GetSettingsService();

            var expected = sut.GetDefaultSettings();
            var actual = await sut.GetCurrentSettingsAsync();

            Assert.True(IsEqual(expected, actual));
        }

        [Fact]
        public async Task RestoreDefaultSettings_SetsDefaultSettings()
        {
            var sut = GetSettingsService();

            var settings = new Settings
            {
                LightBackgroundColor = "#ffffff",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#eeeeee",
                DarkTextColor = "#111111",
            };
            await sut.SaveSettingsAsync(settings);

            await sut.RestoreDefaultSettings();
            var expected = sut.GetDefaultSettings();
            var actual = await sut.GetCurrentSettingsAsync();

            Assert.True(IsEqual(expected, actual));
        }

        [Fact]
        public async Task SaveSettingsAsync_SetCurrentSettings()
        {
            var sut = GetSettingsService();

            var settings = new Settings
            {
                LightBackgroundColor = "#ffffff",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#eeeeee",
                DarkTextColor = "#111111",
            };
            await sut.SaveSettingsAsync(settings);
            var actual = await sut.GetCurrentSettingsAsync();

            Assert.True(IsEqual(actual, settings));
        }

        [Fact]
        public async Task SaveSettingsAsync_SavesCurrentSettingsToLocalDataStore()
        {
            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            var sut = GetSettingsService(localDataStorageServiceStub);

            var settings = new Settings
            {
                LightBackgroundColor = "#ffffff",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#eeeeee",
                DarkTextColor = "#111111",
            };
            await sut.SaveSettingsAsync(settings);
            var actual = await sut.GetCurrentSettingsAsync();

            localDataStorageServiceStub.Verify(r => r.PutSettingsAsync(actual), Times.Once);
        }

        [Fact]
        public async Task SaveSettingsAsync_WhenAutoSyncEnabled_SavesCurrentSettingsOnServer()
        {
            var syncServiceStub = new Mock<ISyncService>();
            var sut = GetSettingsService(syncServiceStub: syncServiceStub);

            var settings = new Settings
            {
                LightBackgroundColor = "#ffffff",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#eeeeee",
                DarkTextColor = "#111111",
                AutoSync = true,
            };
            await sut.SaveSettingsAsync(settings);
            var actual = await sut.GetCurrentSettingsAsync();

            syncServiceStub.Verify(r => r.SaveSettingsAsync(actual), Times.Once);
        }


        bool IsEqual(Settings first, Settings second)
        {
            return first.LightBackgroundColor == second.LightBackgroundColor
                && first.DarkBackgroundColor == second.DarkBackgroundColor
                && first.LightTextColor == second.LightTextColor
                && first.DarkTextColor == second.DarkTextColor;

        }

        private SettingsService GetSettingsService(
            Mock<ILocalDataStorageService> localDataStorageServiceStub = null,
            Mock<ISyncService> syncServiceStub = null)
        {
            if (localDataStorageServiceStub is null)
                localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            if (syncServiceStub is null)
                syncServiceStub = new Mock<ISyncService>();
            return new SettingsService(localDataStorageServiceStub.Object, syncServiceStub.Object);
        }
    }
}
