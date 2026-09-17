namespace CityHallBookingSystem.Models
{
	public class Resource
	{
		public int Id { get; set; }
		public ReservationType Reservation { get; set; }
		public string Comments { get; set; } = string.Empty;
		public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
	}
}
