using System.ComponentModel.DataAnnotations;

namespace CityHallBookingSystem.DTOs
{
	public class CreateCustomerDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required(ErrorMessage = "Email is required.")]
		[RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email is not a valid email address.")]
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
	}
}
