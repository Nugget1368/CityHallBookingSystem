using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Models;

namespace CityHallBookingSystem.Mappings
{
	public class CustomerMapper
	{
		/// <summary>
		/// Maps a Customer entity to a CustomerDto object.
		/// </summary>
		/// <param name="customer">The Customer entity to map.</param>
		/// <returns>The corresponding CustomerDto object.</returns>
		public static CustomerDto ToDto(Customer customer)
		{
			return new CustomerDto
			{
				Id = customer.Id,
				Name = customer.Name,
				Email = customer.Email,
				Phone = customer.Phone
			};
		}
		/// <summary>
		///  Maps a CreateCustomerDto object to a Customer entity.
		/// </summary>
		/// <param name="customerDto">The CreateCustomerDto object to map.</param>
		/// <returns>The corresponding Customer entity.</returns>
		public static Customer ToEntity(CreateCustomerDto customerDto)
		{
			return new Customer
			{
				Name = customerDto.Name,
				Email = customerDto.Email,
				Phone = customerDto.Phone
			};
		}
	}
}
