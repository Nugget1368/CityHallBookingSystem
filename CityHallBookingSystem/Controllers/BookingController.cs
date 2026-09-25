using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Mappings;
using CityHallBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityHallBookingSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BookingController : ControllerBase
	{
		private readonly BookingDbContext _dbContext;

		public BookingController(BookingDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
		{
			var bookings = await _dbContext.Bookings
				.Include(b => b.Customer)
				.Include(b => b.Resource)
				.AsNoTracking()
				.ToListAsync();
			var bookingDtos = bookings
				.Select(BookingMapper.ToDto)
				.ToList();
			return Ok(bookingDtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BookingDto>> GetBooking(int id)
		{
			var booking = await _dbContext.Bookings
				.Include(b => b.Customer)
				.Include(b => b.Resource)
				.AsNoTracking()
				.FirstOrDefaultAsync(b => b.Id == id);
			if (booking == null)
				return NotFound();
			return Ok(BookingMapper.ToDto(booking));
		}

		[HttpPost]
		public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto bookingDto)
		{
			if (bookingDto.StartTime >= bookingDto.EndTime)
				return BadRequest("Start time must be before end time.");

			var customer = await _dbContext.Customers.FindAsync(bookingDto.CustomerId);
			if (customer == null)
				return NotFound($"Customer with ID {bookingDto.CustomerId} not found.");

			var resource = await _dbContext.Resources.FindAsync(bookingDto.ResourceId);
			if (resource == null)
				return NotFound($"Resource with ID {bookingDto.ResourceId} not found.");

			var booking = BookingMapper.ToEntity(bookingDto);

			_dbContext.Bookings.Add(booking);
			await _dbContext.SaveChangesAsync();
			var result = BookingMapper.ToDto(booking);
			return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, result);
		}

		[HttpPut("{id}/cancel")]
		public async Task<IActionResult> CancelBooking(int id)
		{
			var booking = await _dbContext.Bookings.FindAsync(id);
			if (booking == null)
				return NotFound();
			booking.Status = BookingStatus.Cancelled;
			await _dbContext.SaveChangesAsync();
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBooking(int id, UpdateBookingDto bookingDto)
		{
			if (bookingDto.StartTime >= bookingDto.EndTime)
				return BadRequest("Start time must be before end time.");
			var booking = await _dbContext.Bookings.FindAsync(id);
			if (booking == null)
				return NotFound();
			var resource = await _dbContext.Resources.FindAsync(bookingDto.ResourceId);
			if (resource == null)
				return NotFound($"Resource with ID {bookingDto.ResourceId} not found.");
			booking.ResourceId = bookingDto.ResourceId;
			booking.StartTime = bookingDto.StartTime;
			booking.EndTime = bookingDto.EndTime;
			await _dbContext.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBooking(int id)
		{
			var booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id);
			if (booking == null)
				return NotFound();
			_dbContext.Bookings.Remove(booking);
			await _dbContext.SaveChangesAsync();
			return NoContent();
		}
	}
}
