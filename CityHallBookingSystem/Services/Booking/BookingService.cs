

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
		public async Task<ServiceResult<BookingDto>> CreateBooking(CreateBookingDto bookingDto)
		{

			if (bookingDto.StartTime >= bookingDto.EndTime)
				return ServiceResult<BookingDto>.Fail(ServiceError.InvalidTime, $"Start time must be before end time");

			var customer = await _dbContext.Customers.FindAsync(bookingDto.CustomerId);
			if (customer == null)
				return ServiceResult<BookingDto>.Fail(ServiceError.CustomerNotFound, $"Customer with ID {bookingDto.CustomerId} not found.");

			var resource = await _dbContext.Resources.FindAsync(bookingDto.ResourceId);
			if (resource == null)
				return ServiceResult<BookingDto>.Fail(ServiceError.ResourceNotFound, $"Resource with ID {bookingDto.ResourceId} not found.");

			var resourceAlreadyAssigned = await _dbContext.Bookings
				.AnyAsync(b => b.ResourceId == bookingDto.ResourceId);
			if (resourceAlreadyAssigned)
				return ServiceResult<BookingDto>.Fail( ServiceError.ResourceAlreadyAssigned, $"Resource with ID {bookingDto.ResourceId} is already assigned to a booking.");

			var booking = BookingMapper.ToEntity(bookingDto);
			_dbContext.Bookings.Add(booking);
			await _dbContext.SaveChangesAsync();
			var result = BookingMapper.ToDto(booking);
			return ServiceResult<BookingDto>.Ok(result);
		}

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

			return ServiceResult<IEnumerable<BookingDto>>.Ok(bookingDtos);
		}
		public async Task<ServiceResult<bool>> CancelBooking(int id)
		{
			var booking = await _dbContext.Bookings.FindAsync(id);
			if (booking == null)
				return ServiceResult<bool>.Fail(ServiceError.BookingNotFound, $"Booking with ID {id} not found.");
			booking.Status = BookingStatus.Cancelled;
			await _dbContext.SaveChangesAsync();
			return ServiceResult<bool>.Ok(true);
		}

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
