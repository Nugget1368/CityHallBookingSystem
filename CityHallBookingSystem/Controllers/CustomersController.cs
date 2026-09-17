using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
			var customerDtos = customers.Select(c => new CustomerDto
			{
				Id = c.Id,
				Name = c.Name,
				Email = c.Email,
				Phone = c.Phone
			}).ToList();
			return Ok(customerDtos);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
		{
			var customer = await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
			if (customer == null)
				return NotFound();
			var customerDto = new CustomerDto
			{
				Id = customer.Id,
				Name = customer.Name,
				Email = customer.Email,
				Phone = customer.Phone
			};
			return Ok(customerDto);
		}
		[HttpPost]
		public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto customerDto)
		{
			var customer = new Customer
			{
				Name = customerDto.Name,
				Email = customerDto.Email,
				Phone = customerDto.Phone
			};
			_dbContext.Customers.Add(customer);
			await _dbContext.SaveChangesAsync();
			customerDto.Id = customer.Id;
			return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customerDto);
		}
	}
}
