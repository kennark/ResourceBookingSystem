using Data.Repositories.Interfaces;
using Data.Validation.Interfaces;
using Domain.Entities;

namespace Data.Validation;

public class BookingValidator(IBookingRepository bookingRepository) : IBookingValidator
{
    /**
     * Validates time period of given booking compared with other existing bookings.
     * Returns true if is valid
     */
    public async Task<bool> ValidateTimePeriod(Booking booking)
    {
        var existingBookings = await bookingRepository.GetAllBookingsForResource(booking.ResourceId);

        if (existingBookings.Count == 0)
            return true;

        return !existingBookings.Any(otherBooking =>
            booking.StartTime >= otherBooking.StartTime && booking.EndTime <= otherBooking.EndTime || // ([])
            booking.StartTime >= otherBooking.StartTime && booking.StartTime <= otherBooking.EndTime || // ([)]
            booking.EndTime >= otherBooking.StartTime && booking.EndTime <= otherBooking.EndTime || // [(])
            booking.StartTime <= otherBooking.EndTime && booking.EndTime >= otherBooking.EndTime); // [()]
    }
}