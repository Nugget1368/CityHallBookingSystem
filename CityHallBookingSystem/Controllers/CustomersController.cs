using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityHallBookingSystem.Mappings;

namespace CityHallBookingSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CustomersController : ControllerBase
	{
		private readonly BookingDbContext _dbContext;
		public CustomersController(BookingDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
		{
			var customers = await _dbContext.Customers.AsNoTracking().ToListAsync();
			var customerDtos = customers.Select(c => CustomerMapper.ToDto(c)).ToList();
			return Ok(customerDtos);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
		{
			var customer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			if (customer == null)
				return NotFound();
			var customerDto = CustomerMapper.ToDto(customer);
			return Ok(customerDto);
		}
		[HttpPost]
		public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto customerDto)
		{
			var customer = CustomerMapper.ToEntity(customerDto);
			_dbContext.Customers.Add(customer);
			await _dbContext.SaveChangesAsync();
			var result = CustomerMapper.ToDto(customer);
			return CreatedAtAction(nameof(GetCustomer), new { id = result.Id }, result);
		}
	}
}
