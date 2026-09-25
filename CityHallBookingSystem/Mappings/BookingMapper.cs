using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Models;

namespace CityHallBookingSystem.Mappings;
	/*
	 * This class is responsible for mapping Booking entities to BookingDto objects.
	 * It provides a method to convert a Booking object into its corresponding DTO representation.
	 */
	public class BookingMapper
	{
	/// <summary>
	/// Maps a Booking entity to a BookingDto object.
	/// </summary>
	/// <param name="booking">The Booking entity to map.</param>
	/// <returns>The corresponding BookingDto object.</returns>
	public static BookingDto ToDto(Booking booking)
		{
			return new BookingDto
			{
				Id = booking.Id,
				Customer = new CustomerDto
				{
					Id = booking.Customer.Id,
					Name = booking.Customer.Name,
					Email = booking.Customer.Email,
					Phone = booking.Customer.Phone
				},
				Resource = new ResourceDto
				{
					Id = booking.Resource.Id,
					Reservation = booking.Resource.Reservation,
					Comments = booking.Resource.Comments
				},
				StartTime = booking.StartTime,
				EndTime = booking.EndTime,
				Status = booking.Status,
			};
		}
	public static Booking ToEntity(BookingDto bookingDto)
		{
			return new Booking
			{
				Id = bookingDto.Id,
				CustomerId = bookingDto.Customer.Id,
				ResourceId = bookingDto.Resource.Id,
				StartTime = bookingDto.StartTime,
				EndTime = bookingDto.EndTime,
				Status = bookingDto.Status
			};
	}
}