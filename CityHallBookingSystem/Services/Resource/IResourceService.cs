using CityHallBookingSystem.DTOs;

namespace CityHallBookingSystem.Services.Resource
{
	public interface IResourceService
	{
		Task<ServiceResult<IEnumerable<ResourceDto>>> GetResources();
		Task<ServiceResult<ResourceDto>> GetResource(int id);
		Task<ServiceResult<ResourceDto>> CreateResource(CreateResourceDto resourceDto);
		Task<ServiceResult<bool>> DeleteResource(int id);
	}
}
