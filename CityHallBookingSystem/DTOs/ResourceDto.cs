using CityHallBookingSystem.Models;

namespace CityHallBookingSystem.DTOs
{
	public class ResourceDto
	{
		public int Id { get; set; }
		public ReservationType Reservation { get; set; }
		public string Comments { get; set; } = string.Empty;
	}
}
