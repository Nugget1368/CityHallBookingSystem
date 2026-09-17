using CityHallBookingSystem.Data;
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
	}
}
