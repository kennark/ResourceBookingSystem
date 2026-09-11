using Domain.Entities;

namespace Data.Services.Interfaces;

public interface IBookingService
{
    Task<List<Booking>> GetAllBookings();
}