namespace CityHallBookingSystem.Models
{
	public class Resource
	{
		public int Id { get; set; }
		public ReservationType Reservation { get; set; }
		public string Comments { get; set; } = string.Empty;

		public Booking? Booking { get; set; } // Navigation property to Booking - 1:1 relationship
	}
}
