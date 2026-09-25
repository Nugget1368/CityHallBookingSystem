using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Mappings;
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

		[HttpGet("{id}")]
		public async Task<ActionResult<Resource>> GetResource(int id)
		{
			var resource = await _dbContext.Resources
				.FirstOrDefaultAsync(r => r.Id == id);

			if (resource == null)
			{
				return NotFound();
			}

			return Ok(resource);
		}

		[HttpPost]
		public async Task<ActionResult<Resource>> CreateResource(CreateResourceDto resourceDto)
		{
			var resource = ResourceMapper.ToEntity(resourceDto);

			_dbContext.Resources.Add(resource);

			await _dbContext.SaveChangesAsync();

			return CreatedAtAction(nameof(GetResource), new { id = resource.Id }, resource);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteResource(int id)
		{
			var resource = await _dbContext.Resources
				.Include(r => r.Bookings)
				.FirstOrDefaultAsync(r => r.Id == id);
			if (resource == null)
			{
				return NotFound();
			}
			if (resource.Bookings.Any())
			{
				return Conflict("Cannot delete resource with existing bookings.");
			}
			_dbContext.Resources.Remove(resource);
			await _dbContext.SaveChangesAsync();
			return NoContent();
		}
	}
}
