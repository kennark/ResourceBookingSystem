using Bunit;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ResourceBookingSystem.Components.Pages;
using ResourceBookingSystemTests.Components.FakeRepositories;

namespace ResourceBookingSystemTests.Components;

public class BookingsComponentTests : IDisposable
{
    private readonly BunitContext _context = new();

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void BookingsPage_RendersBookingsForResource()
    {
        var resource = new Resource { Id = 7, Name = "Conference Room" };
        var bookings = new List<Booking>
        {
            new()
            {
                Id = 1,
                EmployeeName = "Alice Johnson",
                StartTime = new DateTime(2026, 9, 13, 9, 0, 0),
                EndTime = new DateTime(2026, 9, 13, 10, 0, 0),
                Notes = "Team sync",
                ResourceId = 7,
                Resource = resource
            },
            new()
            {
                Id = 2,
                EmployeeName = "Bob Smith",
                StartTime = new DateTime(2026, 9, 13, 11, 0, 0),
                EndTime = new DateTime(2026, 9, 13, 12, 0, 0),
                Notes = "Client call",
                ResourceId = 7,
                Resource = resource
            }
        };

        _context.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository([resource]));
        _context.Services.AddSingleton<IBookingRepository>(new FakeBookingRepository(bookings));

        var cut = _context.Render<Bookings>(parameters => parameters
            .Add(p => p.ResourceId, 7));

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Bookings for resource Conference Room:", cut.Markup);
            Assert.Contains("Alice Johnson", cut.Markup);
            Assert.Contains("Bob Smith", cut.Markup);
            Assert.Contains("Team sync", cut.Markup);
            Assert.Contains("Client call", cut.Markup);
        });
    }

    [Fact]
    public void BookingsPage_WhenResourceDoesNotExist_ShowsNotFoundMessage()
    {
        _context.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository([]));
        _context.Services.AddSingleton<IBookingRepository>(new FakeBookingRepository([]));

        var cut = _context.Render<Bookings>(parameters => parameters
            .Add(p => p.ResourceId, 99));

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Resource not found.", cut.Markup);
        });
    }

    [Fact]
    public void BookingsPage_ClickCreateNewBooking_NavigatesToNewBookingPage()
    {
        var resource = new Resource { Id = 5, Name = "Workshop" };

        _context.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository([resource]));
        _context.Services.AddSingleton<IBookingRepository>(new FakeBookingRepository([]));

        var cut = _context.Render<Bookings>(parameters => parameters
            .Add(p => p.ResourceId, 5));

        cut.Find("button").Click();

        Assert.Contains("/NewBooking/5", _context.Services.GetRequiredService<NavigationManager>().Uri);
    }
}
