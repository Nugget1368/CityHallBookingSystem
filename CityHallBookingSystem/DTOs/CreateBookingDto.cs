namespace CityHallBookingSystem.DTOs
{
	public class CreateBookingDto
	{
		public int CustomerId { get; set; }

		public int ResourceId { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }
	}
}
