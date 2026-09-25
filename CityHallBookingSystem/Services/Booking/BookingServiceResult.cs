namespace CityHallBookingSystem.Services.Booking
{
	public enum BookingServiceError
	{
		None,
		InvalidTime,
		BookingNotFound,
		CustomerNotFound,
		ResourceNotFound,
		ResourceAlreadyAssigned
	}
	public class BookingServiceResult<T>
	{
		public bool Success { get; init; }
		public T? Data { get; init; }
		public BookingServiceError Error { get; init; }
		public string? Message { get; init; }
		public static BookingServiceResult<T> Ok(T data)
		{
			return new BookingServiceResult<T>
			{
				Success = true,
				Data = data,
				Error = BookingServiceError.None
			};
		}
		public static BookingServiceResult<T> Fail(BookingServiceError error, string message)
		{
			return new BookingServiceResult<T>
			{
				Success = false,
				Error = error,
				Message = message
			};
		}


	}
}
