using Data.Repositories.Interfaces;
using Domain.Entities;

namespace ResourceBookingSystemTests.Components.FakeRepositories;

public class FakeBookingRepository(IEnumerable<Booking> existingBookings) : IBookingRepository
{
    public List<Booking> Bookings { get; } = [.. existingBookings];

    public Task<List<Booking>> GetAllBookingsForResource(int id)
    {
        return Task.FromResult(Bookings.Where(b => b.ResourceId == id).ToList());
    }

    public Task<Booking> AddBooking(Booking booking)
    {
        Bookings.Add(booking);
        return Task.FromResult(booking);
    }
}
