using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Services.Resource;
using Microsoft.AspNetCore.Mvc;


namespace CityHallBookingSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ResourcesController : ControllerBase
	{
		private readonly IResourceService _resourceService;

		public ResourcesController(IResourceService resourceService)
		{
			_resourceService = resourceService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ResourceDto>>> GetResources()
		{
			var result = await _resourceService.GetResources();
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ResourceDto>> GetResource(int id)
		{
			var result = await _resourceService.GetResource(id);
			if(!result.Success)
				return NotFound(result.Message);
			return Ok(result.Data);
		}

		[HttpPost]
		public async Task<ActionResult<ResourceDto>> CreateResource(CreateResourceDto resourceDto)
		{
			var result = await _resourceService.CreateResource(resourceDto);
			if(!result.Success)
				return BadRequest(result.Message);
			return CreatedAtAction(nameof(GetResource), new { id = result.Data!.Id }, result.Data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteResource(int id)
		{
			var result = await _resourceService.DeleteResource(id);
			if (!result.Success)
				return NotFound(result.Message);
			return NoContent();
		}
	}
}
