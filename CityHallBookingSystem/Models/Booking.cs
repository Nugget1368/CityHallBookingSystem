namespace CityHallBookingSystem.Models
{
	public class Booking
	{
		public int Id { get; set; }

		public int CustomerId { get; set; }

		public int ResourceId { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

		public Customer Customer { get; set; } = null!;

		public Resource Resource { get; set; } = null!;
	}
}
