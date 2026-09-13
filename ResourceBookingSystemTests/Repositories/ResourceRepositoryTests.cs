using Data.Repositories;
using Domain;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ResourceBookingSystemTests.Repositories;

public class ResourceRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<DatabaseContext> _options;

    public ResourceRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new DatabaseContext(_options);
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
    }

    [Fact]
    public async Task GetAllResources_And_ActiveFiltering_GetResourceById_AddOrUpdate()
    {
        await using var context = new DatabaseContext(_options);
        var repo = new ResourceRepository(context);

        var r1 = new Resource { Name = "Room A", IsActive = true };
        var r2 = new Resource { Name = "Room B", IsActive = false };

        await repo.AddOrUpdateResource(r1);
        await repo.AddOrUpdateResource(r2);

        var all = await repo.GetAllResources();
        Assert.Contains(all, x => x.Name == "Room A");
        Assert.Contains(all, x => x.Name == "Room B");

        var active = await repo.GetAllActiveResources();
        var item = Assert.Single(active);
        Assert.Equal("Room A", item.Name);

        var fetched = await repo.GetResourceById(r1.Id);
        Assert.NotNull(fetched);
        Assert.Equal("Room A", fetched!.Name);

        // update
        fetched.Name = "Room A Renamed";
        await repo.AddOrUpdateResource(fetched);

        var updated = await repo.GetResourceById(fetched.Id);
        Assert.Equal("Room A Renamed", updated!.Name);
    }
}