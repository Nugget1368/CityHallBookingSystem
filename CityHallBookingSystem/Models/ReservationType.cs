using System.Text.Json.Serialization;

namespace CityHallBookingSystem.Models;

[JsonConverter(typeof(JsonStringEnumConverter<ReservationType>))] //Convert enum to String, do this to save the enum as String in JSON.
public enum ReservationType
{
	InHouse,
	External
}