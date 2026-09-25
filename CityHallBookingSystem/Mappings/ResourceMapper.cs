using CityHallBookingSystem.Models;
using CityHallBookingSystem.DTOs;

namespace CityHallBookingSystem.Mappings
{
	public class ResourceMapper
	{
		/// <summary>
		/// Maps a Resource entity to a ResourceDto object.
		/// </summary>
		/// <param name="resource">The Resource entity to map.</param>
		/// <returns>The corresponding ResourceDto object.</returns>
		public static ResourceDto ToDto(Resource resource)
		{
			return new ResourceDto
			{
				Id = resource.Id,
				Reservation = resource.Reservation,
				Comments = resource.Comments
			};
		}
		/// <summary>
		/// Maps a CreateResourceDto object to a Resource entity.
		/// </summary>
		/// <param name="resourceDto">The CreateResourceDto object to map.</param>
		/// <returns>The corresponding Resource entity.</returns>
		public static Resource ToEntity(CreateResourceDto resourceDto)
		{
			return new Resource
			{
				Reservation = resourceDto.Reservation,
				Comments = resourceDto.Comments
			};
		}
	}
}
