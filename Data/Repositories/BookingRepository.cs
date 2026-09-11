using Data.Repositories.Interfaces;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class BookingRepository(DatabaseContext dbContext) : IBookingRepository
{
    public async Task<List<Booking>> GetAllBookingsForResource(int id)
    {
        return await dbContext.Bookings.Where(booking => booking.ResourceId == id).ToListAsync();
    }

    public async Task<Booking> AddBooking(Booking booking)
    {
        var result = await dbContext.Bookings.AddAsync(booking);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }
}