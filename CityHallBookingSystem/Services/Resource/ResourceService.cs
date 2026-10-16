using CityHallBookingSystem.Data;
using CityHallBookingSystem.DTOs;
using CityHallBookingSystem.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CityHallBookingSystem.Services.Resource
{
	public class ResourceService : IResourceService
	{
		private readonly BookingDbContext _dbContext;

		public ResourceService(BookingDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ServiceResult<ResourceDto>> CreateResource(CreateResourceDto resourceDto)
		{
			try
			{
				var resource = ResourceMapper.ToEntity(resourceDto);
				_dbContext.Resources.Add(resource);
				await _dbContext.SaveChangesAsync();
				return ServiceResult<ResourceDto>.Ok(ResourceMapper.ToDto(resource));
			}
			catch (Exception ex)
			{
				return ServiceResult<ResourceDto>.Fail(ServiceError.UnknownError, $"An error occurred while creating the resource: {ex.Message}");
			}
		}

		public async Task<ServiceResult<bool>> DeleteResource(int id)
		{
			var resource = await _dbContext.Resources.FirstOrDefaultAsync(r => r.Id == id);
			if (resource == null)
				return ServiceResult<bool>.Fail(ServiceError.ResourceNotFound, $"Resource with ID {id} not found.");
			if (resource.Booking != null)
				return ServiceResult<bool>.Fail(ServiceError.ResourceAlreadyAssigned, "Cannot delete resource with existing bookings.");
			_dbContext.Resources.Remove(resource);
			await _dbContext.SaveChangesAsync();
			return ServiceResult<bool>.Ok(true);
		}

		public async Task<ServiceResult<ResourceDto>> GetResource(int id)
		{
			var resource = await _dbContext.Resources.FirstOrDefaultAsync(r => r.Id == id);
			if (resource == null)
				return ServiceResult<ResourceDto>.Fail(ServiceError.ResourceNotFound, $"Resource with ID {id} not found.");
			return ServiceResult<ResourceDto>.Ok(ResourceMapper.ToDto(resource));
		}

		public async Task<ServiceResult<IEnumerable<ResourceDto>>> GetResources()
		{
			var resources = await _dbContext.Resources.ToListAsync();
			return ServiceResult<IEnumerable<ResourceDto>>.Ok(resources.Select(ResourceMapper.ToDto));
		}
	}
}
