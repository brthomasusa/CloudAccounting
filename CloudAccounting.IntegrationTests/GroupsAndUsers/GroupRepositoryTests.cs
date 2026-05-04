using CloudAccounting.Core.Repositories;
using CloudAccounting.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace CloudAccounting.IntegrationTests.GroupsAndUsers
{
    [Collection("SequentialTestCollection")]
    public class GroupRepositoryTests(DatabaseFixture fixture) : IAsyncLifetime
    {
        private readonly AppDbContext _context = fixture.Context!;
        private readonly IMemoryCache? _memoryCache = fixture.MemoryCache;
        private readonly ConfigurationManager? _config = fixture.Config;
        private IGroupRepository _repo => new GroupRepository(_context, _memoryCache!, new NullLogger<GroupRepository>());
        private readonly Func<Task>? _resetDatabase = fixture.ResetDatabase;

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync() => await _resetDatabase!();

        [Fact]
        public async Task RetrieveAllAsync_GroupRepository_ReturnsMultipleGroups()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);

            // Act
            Result<List<GroupsMaster>> result = await _repo.RetrieveAllAsync();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task RetrieveAsync_GroupRepository_ShouldReturn1Row()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int groupId = 1;

            // Act
            Result<GroupsMaster> result = await _repo.RetrieveAsync(groupId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("AppAdmin", result.Value.GroupTitle);
        }

        [Fact]
        public async Task CreateAsync_GroupRepository_CreatesAndReturnsOneGroup()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            GroupsMaster group = new()
            {
                GroupId = 0,
                GroupTitle = "Test Group"
            };

            // Act
            Result<GroupsMaster> result = await _repo.CreateGroupAsync(group);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(group.GroupTitle, result.Value.GroupTitle);
        }

        [Fact]
        public async Task IsUniqueGroupNameForCreate_GroupRepository_ShouldReturnTrue()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            string groupName = "Unique Group Name";

            // Act
            Result<bool> result = await _repo.IsUniqueGroupNameForCreate(groupName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsUniqueGroupNameForCreate_GroupRepository_ShouldReturnFalse()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            string groupName = "AppAdmin";

            // Act
            Result<bool> result = await _repo.IsUniqueGroupNameForCreate(groupName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task IsExistingGroup_GroupRepository_ValidId_ShouldReturnTrue()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int groupId = 1;

            // Act
            Result<bool> result = await _repo.IsValidGroupId(groupId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task IsExistingGroup_GroupRepository_InvalidId_ShouldReturnFalse()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int groupId = 999;

            // Act
            Result<bool> result = await _repo.IsValidGroupId(groupId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task RetrieveAllUserAsync_GroupRepository_ReturnsMultipleUsers()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            int companyCode = 1;

            // Act
            Result<List<User>> result = await _repo.RetrieveAllUserAsync(companyCode);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task RetrieveUserAsync_GroupRepository_ReturnsOneUser()
        {
            // Arrange
            await ReseedTestDb.ReseedTestDbAsync(_context);
            string userId = "hgonzales@btechnical-consulting.com";

            // Act
            Result<User> result = await _repo.RetrieveUserAsync(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
        }

        private static User UserForCreate()
            => new()
            {
                UserId = "ivanka@btechnical-consulting.com",
                CompanyCode = 1,
                CompanyName = "BTechnical Consulting",
                CompanyYear = 2024,
                CompanyMonthId = 1,
                CompanyMonthName = "January",
                GroupId = 3,
                Admin = "N",
                GroupTitle = "User"
            };

        private static User UserForUpdate()
            => new()
            {
                UserId = "dquale@btechnical-consulting.com",
                CompanyCode = 1,
                CompanyName = "BTechnical Consulting",
                CompanyYear = 2025,
                CompanyMonthId = 7,
                CompanyMonthName = "July",
                GroupId = 2,
                Admin = "Y",
                GroupTitle = "CompanyAdmin"
            };
    }
}