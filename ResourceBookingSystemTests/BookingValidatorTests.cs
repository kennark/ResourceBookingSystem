using Data.Validation;
using Domain.Entities;
using ResourceBookingSystemTests.Components.FakeRepositories;

namespace ResourceBookingSystemTests;

public class BookingValidatorTests
{
    [Fact]
    public async Task ValidateTimePeriod_NoExistingBookings_ReturnsTrue()
    {
        var repo = new FakeBookingRepository(new List<Booking>());
        var validator = new BookingValidator(repo);

        var booking = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 9, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 10, 0, 0),
            EmployeeName = "Alice"
        };

        var valid = await validator.ValidateTimePeriod(booking);

        Assert.True(valid);
    }

    [Fact]
    public async Task ValidateTimePeriod_ExistingContainsNew_ReturnsFalse()
    {
        var existing = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 8, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 12, 0, 0),
            EmployeeName = "Bob"
        };

        var repo = new FakeBookingRepository(new List<Booking> { existing });
        var validator = new BookingValidator(repo);

        var booking = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 9, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 10, 0, 0),
            EmployeeName = "Carol"
        };

        var valid = await validator.ValidateTimePeriod(booking);

        Assert.False(valid);
    }

    [Fact]
    public async Task ValidateTimePeriod_StartsInsideExisting_ReturnsFalse()
    {
        var existing = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 9, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 11, 0, 0),
            EmployeeName = "Dave"
        };

        var repo = new FakeBookingRepository(new List<Booking> { existing });
        var validator = new BookingValidator(repo);

        var booking = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 12, 0, 0),
            EmployeeName = "Eve"
        };

        var valid = await validator.ValidateTimePeriod(booking);

        Assert.False(valid);
    }

    [Fact]
    public async Task ValidateTimePeriod_EndsInsideExisting_ReturnsFalse()
    {
        var existing = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 10, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 12, 0, 0),
            EmployeeName = "Frank"
        };

        var repo = new FakeBookingRepository(new List<Booking> { existing });
        var validator = new BookingValidator(repo);

        var booking = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 9, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 11, 0, 0),
            EmployeeName = "Grace"
        };

        var valid = await validator.ValidateTimePeriod(booking);

        Assert.False(valid);
    }

    [Fact]
    public async Task ValidateTimePeriod_NonOverlapping_ReturnsTrue()
    {
        var existing = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 7, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 8, 0, 0),
            EmployeeName = "Hank"
        };

        var repo = new FakeBookingRepository(new List<Booking> { existing });
        var validator = new BookingValidator(repo);

        var booking = new Booking
        {
            ResourceId = 1,
            StartTime = new DateTime(2026, 9, 12, 9, 0, 0),
            EndTime = new DateTime(2026, 9, 12, 10, 0, 0),
            EmployeeName = "Ivy"
        };

        var valid = await validator.ValidateTimePeriod(booking);

        Assert.True(valid);
    }
}