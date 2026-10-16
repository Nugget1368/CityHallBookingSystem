using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CityHallBookingSystem.Services.Customer
{
	public class CustomerService : ICustomerService
	{
		private readonly BookingDbContext _dbContext;
		public CustomerService(BookingDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<ServiceResult<CustomerDto>> CreateCustomer(CreateCustomerDto customerDto)
		{
			var customer = CustomerMapper.ToEntity(customerDto);
			_dbContext.Customers.Add(customer);
			await _dbContext.SaveChangesAsync();
			var result = CustomerMapper.ToDto(customer);
			return ServiceResult<CustomerDto>.Ok(result);
		}

		public async Task<ServiceResult<CustomerDto>> GetCustomer(int id)
		{
			var customer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			if (customer == null)
				return ServiceResult<CustomerDto>.Fail(ServiceError.CustomerNotFound, $"Customer with ID {id} not found.");
			var customerDto = CustomerMapper.ToDto(customer);
			return ServiceResult<CustomerDto>.Ok(customerDto);
		}

		public async Task<ServiceResult<IEnumerable<CustomerDto>>> GetCustomers()
		{
			var customers = await _dbContext.Customers.AsNoTracking().ToListAsync();
			var customerDtos = customers.Select(c => CustomerMapper.ToDto(c)).ToList();
			return ServiceResult<IEnumerable<CustomerDto>>.Ok(customerDtos);
		}
	}
}
