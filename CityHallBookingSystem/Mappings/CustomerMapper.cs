using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Models;

namespace CityHallBookingSystem.Mappings
{
	public class CustomerMapper
	{
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
		public static Customer ToEntity(CustomerDto customerDto)
		{
			return new Customer
			{
				Id = customerDto.Id,
				Name = customerDto.Name,
				Email = customerDto.Email,
				Phone = customerDto.Phone
			};
		}
	}
}
