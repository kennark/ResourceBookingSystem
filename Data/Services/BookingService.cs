using Data.Services.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public class BookingService(DatabaseContext dbContext) : IBookingService
{
    public async Task<List<Booking>> GetAllBookings()
    {
        return await dbContext.Bookings.ToListAsync();
    }
}