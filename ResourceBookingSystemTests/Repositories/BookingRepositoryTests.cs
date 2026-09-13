using Data.Repositories;
using Domain;
using Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ResourceBookingSystemTests.Repositories;

public class BookingRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<DatabaseContext> _options;

    public BookingRepositoryTests()
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
    public async Task AddBooking_And_GetAllBookingsForResource()
    {
        await using var context = new DatabaseContext(_options);
        var repo = new BookingRepository(context);

        var resource = new Resource { Name = "Conference", IsActive = true };
        context.Resources.Add(resource);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var b1 = new Booking
        {
            ResourceId = resource.Id,
            EmployeeName = "Alice",
            StartTime = new DateTime(2026, 9, 12, 9, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 10, 0, 0)
        };

        var added = await repo.AddBooking(b1);
        Assert.NotNull(added);
        Assert.Equal(resource.Id, added.ResourceId);

        var list = await repo.GetAllBookingsForResource(resource.Id);
        var item = Assert.Single(list);
        Assert.Equal("Alice", item.EmployeeName);
    }
}