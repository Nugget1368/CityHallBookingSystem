using CityHallBookingSystem.Models;

namespace CityHallBookingSystem.DTOs
{
	public class BookingDto
	{
		public int Id { get; set; }

		public int CustomerId { get; set; }

		public int ResourceId { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public BookingStatus Status { get; set; }
	}
}
