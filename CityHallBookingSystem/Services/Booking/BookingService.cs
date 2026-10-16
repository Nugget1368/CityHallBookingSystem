

using CityHallBookingSystem.Controllers;
using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Mappings;
using CityHallBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CityHallBookingSystem.Services.Booking
{
	public class BookingService : IBookingService
	{
		private readonly BookingDbContext _dbContext;

		public BookingService(BookingDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		/// <summary>
		/// Creates a new booking in the system. Validates the booking times, checks if the customer and resource exist, and ensures that the resource is not already assigned to another booking. If all validations pass, the booking is created and saved to the database.
		/// </summary>
		/// <param name="bookingDto">The booking data transfer object containing the details of the booking to be created.</param>
		/// <returns>A ServiceResult containing the created BookingDto if successful, or an error if the creation failed.</returns>
		public async Task<ServiceResult<BookingDto>> CreateBooking(CreateBookingDto bookingDto)
		{
			// Validate the booking times
			if (bookingDto.StartTime >= bookingDto.EndTime)
				return ServiceResult<BookingDto>.Fail(ServiceError.InvalidTime, $"Start time must be before end time");

			// Check if the customer exists
			var customer = await _dbContext.Customers.FindAsync(bookingDto.CustomerId);
			if (customer == null)
				return ServiceResult<BookingDto>.Fail(ServiceError.CustomerNotFound, $"Customer with ID {bookingDto.CustomerId} not found.");

			// Check if the resource exists
			var resource = await _dbContext.Resources.FindAsync(bookingDto.ResourceId);
			if (resource == null)
				return ServiceResult<BookingDto>.Fail(ServiceError.ResourceNotFound, $"Resource with ID {bookingDto.ResourceId} not found.");
			
			// Check if the resource is already assigned to another booking
			var resourceAlreadyAssigned = await _dbContext.Bookings
				.AnyAsync(b => b.ResourceId == bookingDto.ResourceId);
			if (resourceAlreadyAssigned)
				return ServiceResult<BookingDto>.Fail( ServiceError.ResourceAlreadyAssigned, $"Resource with ID {bookingDto.ResourceId} is already assigned to a booking.");

			// Create the booking
			var booking = BookingMapper.ToEntity(bookingDto);
			_dbContext.Bookings.Add(booking);
			await _dbContext.SaveChangesAsync();
			var result = BookingMapper.ToDto(booking);
			return ServiceResult<BookingDto>.Ok(result);
		}

		/// <summary>
		/// Fetch a booking by its ID. If the booking exists, it returns the booking details; otherwise, it returns an error indicating that the booking was not found.
		/// </summary>
		/// <param name="id">The ID of the booking to fetch.</param>
		/// <returns>A ServiceResult containing the BookingDto if found, or an error if not found.</returns>
		public async Task<ServiceResult<BookingDto>> GetBooking(int id)
		{
			var booking = await _dbContext.Bookings
				.Include(b => b.Customer)
				.Include(b => b.Resource)
				.AsNoTracking()
				.FirstOrDefaultAsync(b => b.Id == id);
			if (booking == null)
				return ServiceResult<BookingDto>.Fail(ServiceError.BookingNotFound, $"Booking with ID {id} not found.");
			return ServiceResult<BookingDto>.Ok(BookingMapper.ToDto(booking));
		}

		public async Task<ServiceResult<IEnumerable<BookingDto>>> GetBookings()
		{
			var bookings = await _dbContext.Bookings
				.Include(b => b.Customer)
				.Include(b => b.Resource)
				.AsNoTracking()
				.ToListAsync();

			var bookingDtos = bookings
				.Select(BookingMapper.ToDto)
				.ToList();

			var result = ServiceResult<IEnumerable<BookingDto>>.Ok(bookingDtos);
			return result;
		}
		/// <summary>
		/// Edit existing booking status to Cancelled. If the booking exists, it updates the status and saves the changes; otherwise, it returns an error indicating that the booking was not found.
		/// </summary>
		/// <param name="id">The ID of the booking to cancel.</param>
		/// <returns>A ServiceResult indicating whether the cancellation was successful or if an error occurred.</returns>
		public async Task<ServiceResult<bool>> CancelBooking(int id)
		{
			var booking = await _dbContext.Bookings.FindAsync(id);
			if (booking == null)
				return ServiceResult<bool>.Fail(ServiceError.BookingNotFound, $"Booking with ID {id} not found.");
			booking.Status = BookingStatus.Cancelled;
			await _dbContext.SaveChangesAsync();
			return ServiceResult<bool>.Ok(true);
		}
		/// <summary>
		/// Update an existing booking's details. Validates the new booking times, checks if the booking and resource exist, and updates the booking if all validations pass. If the booking does not exist, it returns an error indicating that the booking was not found.
		/// </summary>
		/// <param name="id">The ID of the booking to update.</param>
		/// <param name="bookingDto">The updated booking details.</param>
		/// <returns>A ServiceResult indicating whether the update was successful or if an error occurred.</returns>
		public async Task<ServiceResult<bool>> UpdateBooking(int id, UpdateBookingDto bookingDto)
		{
			if (bookingDto.StartTime >= bookingDto.EndTime)
				return ServiceResult<bool>.Fail(ServiceError.InvalidTime, $"Start time must be before end time.");
			
			var booking = await _dbContext.Bookings.FindAsync(id);
			if (booking == null)
				return ServiceResult<bool>.Fail(ServiceError.BookingNotFound, $"Booking with ID {id} not found.");
			
			var resource = await _dbContext.Resources.FindAsync(bookingDto.ResourceId);
			if (resource == null)
				return ServiceResult<bool>.Fail(ServiceError.ResourceNotFound, $"Resource with ID {bookingDto.ResourceId} not found.");
			
			booking.ResourceId = bookingDto.ResourceId;
			booking.StartTime = bookingDto.StartTime;
			booking.EndTime = bookingDto.EndTime;
			await _dbContext.SaveChangesAsync();
			return ServiceResult<bool>.Ok(true);
		}
		/// <summary>
		/// Delete an existing booking from the system. If the booking exists, it removes the booking and saves the changes; otherwise, it returns an error indicating that the booking was not found.
		/// </summary>
		/// <param name="id">The ID of the booking to delete.</param>
		/// <returns>A ServiceResult indicating whether the deletion was successful or if an error occurred.</returns>
		public async Task<ServiceResult<bool>> DeleteBooking(int id)
		{
			var booking = await _dbContext.Bookings.FindAsync(id);
			if (booking == null)
				return ServiceResult<bool>.Fail(ServiceError.BookingNotFound, $"Booking with ID {id} not found.");
			
			_dbContext.Bookings.Remove(booking);
			await _dbContext.SaveChangesAsync();
			return ServiceResult<bool>.Ok(true);
		}
	}
}
