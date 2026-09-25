using CityHallBookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CityHallBookingSystem.Data
{
	public class BookingDbContext : DbContext
	{
		public BookingDbContext(DbContextOptions<BookingDbContext> options)
			: base(options)
		{
		}

		// Create Tables: Customers, Resources, Bookings
		public DbSet<Customer> Customers { get; set; }

		public DbSet<Resource> Resources { get; set; }

		public DbSet<Booking> Bookings { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Resource is Enum but save as string in SQL
			modelBuilder.Entity<Resource>()
				.Property(r => r.Reservation)
				.HasConversion<string>();

			// Booking.Status is an enum but is saved as a string in SQL
			modelBuilder.Entity<Booking>()
				.Property(b => b.Status)
				.HasConversion<string>();

			modelBuilder.Entity<Booking>()
				.HasOne(b => b.Resource)
				.WithOne(r => r.Booking)
				.HasForeignKey<Booking>(b => b.ResourceId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
