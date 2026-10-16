using Microsoft.AspNetCore.Mvc;

namespace CityHallBookingSystem.Services
{
	public enum ServiceError
	{
		None,
		InvalidTime,
		BookingNotFound,
		CustomerNotFound,
		ResourceNotFound,
		ResourceAlreadyAssigned
	}
	/// <summary>
	/// Result of a booking service operation, indicating success or failure and providing relevant data or error information.
	/// </summary>
	/// <typeparam name="T">The type of data returned by the booking service operation.</typeparam>
	public class ServiceResult<T>
	{
		public bool Success { get; init; }
		public T? Data { get; init; }
		public ServiceError Error { get; init; }
		public string? Message { get; init; }
		/// <summary>
		/// Successful result with data.
		/// </summary>
		/// <param name="data">The data returned by the booking service operation.</param>
		/// <returns>A BookingServiceResult representing a successful operation.</returns>
		public static ServiceResult<T> Ok(T data)
		{
			return new ServiceResult<T>
			{
				Success = true,
				Data = data,
				Error = ServiceError.None
			};
		}

		public static ServiceResult<T> Fail(ServiceError error, string message)
		{
			return new ServiceResult<T>
			{
				Success = false,
				Error = error,
				Message = message
			};
		}
	}
}
