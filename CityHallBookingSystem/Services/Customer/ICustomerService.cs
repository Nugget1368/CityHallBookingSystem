using CityHallBookingSystem.DTOs;

namespace CityHallBookingSystem.Services.Customer
{
	public interface ICustomerService
	{
		Task<ServiceResult<IEnumerable<CustomerDto>>> GetCustomers();
		Task<ServiceResult<CustomerDto>> GetCustomer(int id);
		Task<ServiceResult<CustomerDto>> CreateCustomer(
			CreateCustomerDto customerDto);
		//Task<ServiceResult<bool>> UpdateCustomer(
		//	int id,
		//	UpdateCustomerDto customerDto);
		//Task<ServiceResult<bool>> DeleteCustomer(int id);
	}
}
