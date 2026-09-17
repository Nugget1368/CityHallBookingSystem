using CityHallBookingSystem.Models;

namespace CityHallBookingSystem.DTOs
{
	public class BookingDto
	{
		public int Id { get; set; }

		public CustomerDto Customer { get; set; } = null!;

		public ResourceDto Resource { get; set; } = null!;

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public BookingStatus Status { get; set; }
	}
}
