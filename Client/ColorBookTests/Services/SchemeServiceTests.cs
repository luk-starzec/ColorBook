using ColorBook.Models;
using ColorBook.Services;
using ColorBook.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ColorBookTests.Services
{
    public class SchemeServiceTests
    {
        [Fact]
        public void DuplicateScheme_CreatesSchemeWithDifferentId()
        {
            var colorScheme = new ColorScheme
            {
                Id = Guid.NewGuid()
            };

            var sut = GetSchemeService();
            var actual = sut.DuplicateScheme(colorScheme);

            Assert.NotEqual(colorScheme.Id, actual.Id);
        }

        [Fact]
        public void DuplicateScheme_CreatesSchemeWithSameColors()
        {
            var colorScheme = new ColorScheme
            {
                Colors = new List<NamedColor>
                {
                    new NamedColor("first", "#000000"),
                    new NamedColor("second", "#ffffff"),
                }
            };

            var sut = GetSchemeService();
            var actual = sut.DuplicateScheme(colorScheme);

            Assert.Collection(actual.Colors,
                item => Assert.Equal(item, colorScheme.GetColor(0)),
                item => Assert.Equal(item, colorScheme.GetColor(1))
            );
        }

        [Fact]
        public async Task AddScheme_WhenSchemeWithEmptyName_CreatesSchemeWithDefaultName()
        {
            var colorScheme = new ColorScheme
            {
                Name = string.Empty,
            };

            var sut = GetSchemeService();
            var actual = await sut.AddScheme(colorScheme);

            Assert.NotEqual(actual.Name, string.Empty);
        }

        [Fact]
        public async Task AddScheme_CreatesSchemeWithNewId()
        {
            var colorScheme = new ColorScheme
            {
                Id = Guid.Empty,
            };

            var sut = GetSchemeService();
            var actual = await sut.AddScheme(colorScheme);

            Assert.NotEqual(actual.Id, Guid.Empty);
        }

        [Fact]
        public async Task AddScheme_AddsSchemeToLocalDataStore()
        {
            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            var sut = GetSchemeService(localDataStorageServiceStub);

            var colorScheme = new ColorScheme();
            var result = await sut.AddScheme(colorScheme);

            localDataStorageServiceStub.Verify(r => r.PutColorSchemeAsync(colorScheme), Times.Once);
        }

        [Fact]
        public async Task AddScheme_WhenSyncAvailable_AddsSchemeOnServer()
        {
            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));
            var sut = GetSchemeService(syncServiceStub: syncServiceStub);

            var colorScheme = new ColorScheme();
            var result = await sut.AddScheme(colorScheme);

            syncServiceStub.Verify(r => r.UpdateScheme(colorScheme), Times.Once);
        }

        [Fact]
        public async Task RemoveScheme_RemovesSchemeFormLocalDataStore()
        {
            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            var sut = GetSchemeService(localDataStorageServiceStub);

            var id = Guid.NewGuid();
            await sut.RemoveScheme(id);

            localDataStorageServiceStub.Verify(r => r.RemoveDeletedColorScheme(id), Times.Once);
        }

        [Fact]
        public async Task RemoveScheme_WhenSyncAvailable_RemovesSchemeFormServer()
        {
            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));
            var sut = GetSchemeService(syncServiceStub: syncServiceStub);

            var id = Guid.NewGuid();
            await sut.RemoveScheme(id);

            syncServiceStub.Verify(r => r.DeleteScheme(id), Times.Once);
        }

        [Fact]
        public async Task RemoveScheme_WhenSyncNotAvailable_AddsDeletedSchemeToLocalDataStore()
        {
            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(false));
            var sut = GetSchemeService(localDataStorageServiceStub, syncServiceStub);

            var id = Guid.NewGuid();
            await sut.RemoveScheme(id);

            localDataStorageServiceStub.Verify(r => r.PutDeletedColorSchemeAsync(id), Times.Once);

        }

        [Fact]
        public async Task GetSchemes_WhenServerAvailableAndNewerDeletedSchemesInDataStore_RemovesDeletedSchemesFromServerResult()
        {
            var id = Guid.NewGuid();
            var fromServer = new ColorScheme[]{
                new()
                {
                    Id = id,
                    LastUpdate = DateTime.Now.AddDays(-1),
                },
            };
            var deleted = new DeletedColorScheme[]{
                new DeletedColorScheme(id, DateTime.Now),
            };

            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            localDataStorageServiceStub.Setup(r => r.GetDeletedColorSchemesAsync())
                .Returns(Task.FromResult(deleted));

            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));
            syncServiceStub.Setup(r => r.LoadSchemesAsync())
                .Returns(Task.FromResult(fromServer));

            var sut = GetSchemeService(localDataStorageServiceStub, syncServiceStub);

            var actual = await sut.GetSchemes();

            Assert.Empty(actual);
        }

        [Fact]
        public async Task GetSchemes_WhenServerAvailableAndDeletedschemesInDataStore_RemovesDeletedSchemesFromLocalDataStore()
        {
            var id = Guid.NewGuid();
            var fromServer = new ColorScheme[]{
                new() {Id = id},
            };
            var deleted = new DeletedColorScheme[] {
                new DeletedColorScheme(id, DateTime.Now)
            };

            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            localDataStorageServiceStub.Setup(r => r.GetDeletedColorSchemesAsync())
                .Returns(Task.FromResult(deleted));

            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));
            syncServiceStub.Setup(r => r.LoadSchemesAsync())
                .Returns(Task.FromResult(fromServer));

            var sut = GetSchemeService(localDataStorageServiceStub, syncServiceStub);

            var result = await sut.GetSchemes();

            localDataStorageServiceStub.Verify(r => r.RemoveDeletedColorScheme(id), Times.Once);
        }

        [Fact]
        public async Task GetSchemes_WhenSyncAvailable_ReturnsSchemeFormServer()
        {
            var fromServer = new ColorScheme[]{
                new() {Id = Guid.NewGuid()},
            };

            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));
            syncServiceStub.Setup(r => r.LoadSchemesAsync())
                .Returns(Task.FromResult(fromServer));

            var sut = GetSchemeService(localDataStorageServiceStub, syncServiceStub);

            var actual = await sut.GetSchemes();

            Assert.Equal(fromServer.Length, actual.Length);
            Assert.All(actual, r => fromServer.Select(rr => rr.Id).Contains(r.Id));
        }

        [Fact]
        public async Task GetSchemes_WhenSyncNotAvailable_ReturnsSchemeFormLocalDataStorage()
        {
            var fromLocal = new ColorScheme[]{
                new() {Id = Guid.NewGuid()},
            };

            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            localDataStorageServiceStub.Setup(r => r.GetColorSchemesAsync())
                .Returns(Task.FromResult(fromLocal));
            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(false));
            var sut = GetSchemeService(localDataStorageServiceStub, syncServiceStub);

            var actual = await sut.GetSchemes();

            Assert.Equal(fromLocal.Length, actual.Length);
            Assert.All(actual, r => fromLocal.Select(rr => rr.Id).Contains(r.Id));
        }

        [Fact]
        public async Task GetSchemes_WhenServerLastUpdateIsNewer_ReturnsSchemeFormServer()
        {
            var id = Guid.NewGuid();
            var fromServer = new ColorScheme[]{
                new()
                {
                    Id = id,
                    LastUpdate = DateTime.Now,
                    Name = "from server",
                },
            };
            var fromLocal = new ColorScheme[]{
                new()
                {
                    Id = id,
                    LastUpdate = DateTime.Now.AddDays(-1),
                    Name = "from local",
                },
            };

            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            localDataStorageServiceStub.Setup(r => r.GetColorSchemesAsync())
                .Returns(Task.FromResult(fromLocal));

            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));
            syncServiceStub.Setup(r => r.LoadSchemesAsync())
                .Returns(Task.FromResult(fromServer));

            var sut = GetSchemeService(localDataStorageServiceStub, syncServiceStub);

            var actual = await sut.GetSchemes();

            Assert.Equal(fromServer[0].Name, actual[0].Name);
        }

        [Fact]
        public async Task GetSchemes_WhenServerLastUpdateIsOlder_ReturnsSchemeFormLocalDataStorage()
        {
            var id = Guid.NewGuid();
            var fromServer = new ColorScheme[]{
                new()
                {
                    Id = id,
                    LastUpdate = DateTime.Now.AddDays(-1),
                    Name = "from server",
                },
            };
            var fromLocal = new ColorScheme[]{
                new()
                {
                    Id = id,
                    LastUpdate = DateTime.Now,
                    Name = "from local",
                },
            };

            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            localDataStorageServiceStub.Setup(r => r.GetColorSchemesAsync())
                .Returns(Task.FromResult(fromLocal));

            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));
            syncServiceStub.Setup(r => r.LoadSchemesAsync())
                .Returns(Task.FromResult(fromServer));

            var sut = GetSchemeService(localDataStorageServiceStub, syncServiceStub);

            var actual = await sut.GetSchemes();

            Assert.Equal(fromLocal[0].Name, actual[0].Name);

        }

        [Fact]
        public async Task GetSchemes_WhenSyncAvailable_UpdatesSchemesOnServer()
        {
            var fromLocal = new ColorScheme[]{
                new() {Id = Guid.NewGuid()},
            };

            var localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            localDataStorageServiceStub.Setup(r => r.GetColorSchemesAsync())
                .Returns(Task.FromResult(fromLocal));
            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));

            var sut = GetSchemeService(localDataStorageServiceStub, syncServiceStub);

            var result = await sut.GetSchemes();

            syncServiceStub.Verify(r => r.UpdateScheme(fromLocal[0]), Times.Once);
        }

        [Fact]
        public async Task GetSchemes_WhenSyncAvailable_AddsSchemesOnServer()
        {
            var syncServiceStub = new Mock<ISyncService>();
            syncServiceStub.Setup(r => r.GetSyncAvailabilityAsync())
                .Returns(Task.FromResult(true));
            var sut = GetSchemeService(syncServiceStub: syncServiceStub);

            var result = await sut.GetSchemes();

            syncServiceStub.Verify(r => r.SaveSchemesAsync(It.IsAny<ColorScheme[]>()), Times.Once);
        }

        private SchemeService GetSchemeService(
            Mock<ILocalDataStorageService> localDataStorageServiceStub = null,
            Mock<ISyncService> syncServiceStub = null)
        {
            if (localDataStorageServiceStub is null)
                localDataStorageServiceStub = new Mock<ILocalDataStorageService>();
            if (syncServiceStub is null)
                syncServiceStub = new Mock<ISyncService>();

            return new SchemeService(localDataStorageServiceStub.Object, syncServiceStub.Object);
        }

    }
}
