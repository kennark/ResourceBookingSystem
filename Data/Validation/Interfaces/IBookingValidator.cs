using Domain.Entities;

namespace Data.Validation.Interfaces;

public interface IBookingValidator
{
    Task<bool> ValidateTimePeriod(Booking booking);
}