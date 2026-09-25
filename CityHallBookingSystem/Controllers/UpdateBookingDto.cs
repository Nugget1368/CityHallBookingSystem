namespace CityHallBookingSystem.Controllers
{
	public class UpdateBookingDto
	{
		public int ResourceId { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }
	}
}
