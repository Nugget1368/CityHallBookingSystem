using CityHallBookingSystem.Models;

namespace CityHallBookingSystem.DTOs
{
	public class CreateResourceDto
	{
		public ReservationType Reservation {  get; set; }

		public string Comments { get; set; } = string.Empty;
	}
}
