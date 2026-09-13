using Bunit;
using Data.Repositories.Interfaces;
using Data.Validation;
using Data.Validation.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ResourceBookingSystem.Components.Pages;
using ResourceBookingSystemTests.Components.FakeRepositories;

namespace ResourceBookingSystemTests.Components;

public class NewBookingTests : IDisposable
{
    private readonly BunitContext _context = new();

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void NewBooking_LoadsResourceAndShowsForm()
    {
        var resource = new Resource { Id = 7, Name = "Conference Room" };
        var bookingRepo = new FakeBookingRepository([]);

        _context.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository([resource]));
        _context.Services.AddSingleton<IBookingRepository>(bookingRepo);
        _context.Services.AddSingleton<IBookingValidator>(new BookingValidator(bookingRepo));

        var cut = _context.Render<NewBooking>(parameters => parameters
            .Add(p => p.ResourceId, 7));

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Create a new booking for Resource Conference Room", cut.Markup);
            Assert.Contains("Employee Name", cut.Markup);
            Assert.Contains("Start Time", cut.Markup);
            Assert.Contains("End Time", cut.Markup);
            Assert.Contains("Create New Booking", cut.Markup);
        });
    }

    [Fact]
    public void NewBooking_WhenResourceDoesNotExist_ShowsNotFoundMessage()
    {
        var bookingRepo = new FakeBookingRepository([]);

        _context.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository([]));
        _context.Services.AddSingleton<IBookingRepository>(bookingRepo);
        _context.Services.AddSingleton<IBookingValidator>(new BookingValidator(bookingRepo));

        var cut = _context.Render<NewBooking>(parameters => parameters
            .Add(p => p.ResourceId, 99));

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Resource not found.", cut.Markup);
        });
    }

    [Fact]
    public void NewBooking_Submit_WithValidBooking_AddsBookingAndNavigatesToBookingPage()
    {
        var resource = new Resource { Id = 5, Name = "Workshop" };
        var bookingRepo = new FakeBookingRepository([]);

        _context.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository([resource]));
        _context.Services.AddSingleton<IBookingRepository>(bookingRepo);
        _context.Services.AddSingleton<IBookingValidator>(new BookingValidator(bookingRepo));

        var cut = _context.Render<NewBooking>(parameters => parameters
            .Add(p => p.ResourceId, 5));

        cut.WaitForAssertion(() => Assert.Contains("Create New Booking", cut.Markup));

        var inputs = cut.FindAll("input");
        inputs[0].Change("Alice Johnson");
        var dateInputs = cut.FindAll("input[type='datetime-local']");
        dateInputs[0].Change("2026-09-13T09:00");
        dateInputs = cut.FindAll("input[type='datetime-local']");
        dateInputs[1].Change("2026-09-13T10:00");
        cut.Find("textarea").Change("Team sync");

        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            var item = Assert.Single(bookingRepo.Bookings);
            Assert.Equal("Alice Johnson", item.EmployeeName);
            Assert.Contains("/Booking/5", _context.Services.GetRequiredService<NavigationManager>().Uri);
        });
    }

    [Fact]
    public void NewBooking_Submit_WithOverlappingBooking_ShowsError()
    {
        var resource = new Resource { Id = 8, Name = "Meeting Room" };
        var existingBooking = new Booking
        {
            Id = 1,
            EmployeeName = "Existing User",
            StartTime = new DateTime(2026, 9, 13, 9, 0, 0),
            EndTime = new DateTime(2026, 9, 13, 10, 0, 0),
            Notes = "Existing block",
            ResourceId = 8,
            Resource = resource
        };
        var bookingRepo = new FakeBookingRepository([existingBooking]);

        _context.Services.AddSingleton<IResourceRepository>(new FakeResourceRepository([resource]));
        _context.Services.AddSingleton<IBookingRepository>(bookingRepo);
        _context.Services.AddSingleton<IBookingValidator>(new BookingValidator(bookingRepo));

        var cut = _context.Render<NewBooking>(parameters => parameters
            .Add(p => p.ResourceId, 8));

        cut.WaitForAssertion(() => Assert.Contains("Create New Booking", cut.Markup));

        var inputs = cut.FindAll("input");
        inputs[0].Change("New User");
        var dateInputs = cut.FindAll("input[type='datetime-local']");
        dateInputs[0].Change("2026-09-13T09:30");
        dateInputs = cut.FindAll("input[type='datetime-local']");
        dateInputs[1].Change("2026-09-13T10:30");

        cut.Find("form").Submit();

        cut.WaitForAssertion(() =>
        {
            Assert.Contains("Time period is overlapping with existing bookings.", cut.Markup);
            Assert.DoesNotContain("/Booking/8", _context.Services.GetRequiredService<NavigationManager>().Uri);
        });
    }
}
