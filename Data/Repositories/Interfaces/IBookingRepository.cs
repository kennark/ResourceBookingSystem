using Domain.Entities;

namespace Data.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllBookingsForResource(int id);

    Task<Booking> AddBooking(Booking booking);
}