using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
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
		public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
		{
		//	var bookings = await _dbContext.Bookings
		//.Include(b => b.Customer)
		//.Include(b => b.Resource)
		//.ToListAsync();
			var bookings = await _dbContext.Bookings
				.ToListAsync();
			return Ok(bookings);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Booking>> GetBooking(int id)
		{
			var booking = await _dbContext.Bookings.FirstOrDefaultAsync(id);
			if (booking == null)
				return NotFound();
			return Ok(booking);
		}

		[HttpPost]
		public async Task<ActionResult<Booking>> CreateBooking(CreateBookingDto bookingDto)
		{
			if (bookingDto.StartTime >= bookingDto.EndTime)
				return BadRequest("Start time must be before end time.");

			var customer = await _dbContext.Customers.FindAsync(bookingDto.CustomerId);
			if (customer == null)
				return NotFound($"Customer with ID {bookingDto.CustomerId} not found.");

			var resource = await _dbContext.Resources.FindAsync(bookingDto.ResourceId);
			if (resource == null)
				return NotFound($"Resource with ID {bookingDto.ResourceId} not found.");

			var booking = new Booking
			{
				CustomerId = bookingDto.CustomerId,
				ResourceId = bookingDto.ResourceId,
				StartTime = bookingDto.StartTime,
				EndTime = bookingDto.EndTime,
				Status = BookingStatus.Confirmed
			};

			_dbContext.Bookings.Add(booking);
			await _dbContext.SaveChangesAsync();
			return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
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
	}
}
