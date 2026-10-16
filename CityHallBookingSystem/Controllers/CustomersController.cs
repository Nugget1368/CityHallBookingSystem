using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityHallBookingSystem.Mappings;
using CityHallBookingSystem.Services.Customer;

namespace CityHallBookingSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CustomersController : ControllerBase
	{
		private readonly ICustomerService _customerService;

		public CustomersController(ICustomerService customerService)
		{
			_customerService = customerService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
		{
			var result = await _customerService.GetCustomers();
			if (!result.Success)
				return BadRequest(result.Message);
			return Ok(result.Data);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
		{
			var result = await _customerService.GetCustomer(id);
			if (!result.Success)
				return NotFound(result.Message);
			return Ok(result.Data);
		}
		[HttpPost]
		public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto customerDto)
		{
			var result = await _customerService.CreateCustomer(customerDto);
			if (!result.Success)
				return BadRequest(result.Message);
			return Ok(result.Data);
		}
	}
}
