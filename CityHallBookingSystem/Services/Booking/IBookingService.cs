using CityHallBookingSystem.Controllers;
using CityHallBookingSystem.DTOs;

namespace CityHallBookingSystem.Services.Booking
{
	public interface IBookingService
	{
		Task<ServiceResult<IEnumerable<BookingDto>>> GetBookings();

		Task<ServiceResult<BookingDto>> GetBooking(int id);

		Task<ServiceResult<BookingDto>> CreateBooking(
			CreateBookingDto bookingDto);

		Task<ServiceResult<bool>> UpdateBooking(
			int id,
			UpdateBookingDto bookingDto);

		Task<ServiceResult<bool>> CancelBooking(int id);

		Task<ServiceResult<bool>> DeleteBooking(int id);
	}
}
