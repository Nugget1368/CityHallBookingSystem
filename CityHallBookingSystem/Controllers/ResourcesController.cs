using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CityHallBookingSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ResourcesController : ControllerBase
	{
		private readonly BookingDbContext _dbContext;

		public ResourcesController(BookingDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
		{
			var resources = await _dbContext.Resources.ToListAsync();
			return Ok(resources);
		}
		[HttpPost]
		public async Task<ActionResult<Resource>> CreateResource(CreateResourceDto resourceDto)
		{
			var resource = new Resource()
			{
				Reservation = resourceDto.Reservation,
				Comments = resourceDto.Comments,
			};

			_dbContext.Resources.Add(resource);

			await _dbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(GetResources), new { id = resource.Id }, resource);
		}
	}
}
