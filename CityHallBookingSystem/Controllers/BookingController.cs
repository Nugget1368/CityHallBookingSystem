using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Handlers;
using CityHallBookingSystem.Mappings;
using CityHallBookingSystem.Models;
using CityHallBookingSystem.Services;
using CityHallBookingSystem.Services.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityHallBookingSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BookingController : ControllerBase
	{
		private readonly IBookingService _bookingService;

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
		{
			var result = await _bookingService.GetBookings();
			return Ok(result.Data);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BookingDto>> GetBooking(int id)
		{
			var result = await _bookingService.GetBooking(id);
			if(!result.Success)
				return NotFound(result.Message);
			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto bookingDto)
		{
			var result = await _bookingService.CreateBooking(bookingDto);
			if (!result.Success)
				return BadRequest(result.Message);
			return CreatedAtAction(nameof(GetBooking), new { id = result.Data!.Id }, result.Data);
		}

		[HttpPut("{id}/cancel")]
		public async Task<IActionResult> CancelBooking(int id)
		{
			var booking = await _bookingService.CancelBooking(id);
			if(!booking.Success)
				return NotFound(booking.Message);
			return Ok(booking);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBooking(int id, UpdateBookingDto bookingDto)
		{
			var result = await _bookingService.UpdateBooking(id, bookingDto);
			if (!result.Success)
				return NotFound(result.Message);
			return Ok(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBooking(int id)
		{
			var result = await _bookingService.DeleteBooking(id);
			if (!result.Success)
				return NotFound(result.Message);
			return NoContent();
		}
	}
}
