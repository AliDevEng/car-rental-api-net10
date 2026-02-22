using Microsoft.EntityFrameworkCore;
using CarRental.Core.Entities;

namespace CarRental.Infrastructure.Data;

public class CarRentalDbContext : DbContext
{
    public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<CarsCategory> CarsCategories { get; set; } = null!;
    public DbSet<Rental> Rentals { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed categories - Budget, Economy, SUV, Transport
        modelBuilder.Entity<CarsCategory>().HasData(
            new CarsCategory { Id = 1, Name = "Budget" },
            new CarsCategory { Id = 2, Name = "Economy" },
            new CarsCategory { Id = 3, Name = "SUV" },
            new CarsCategory { Id = 4, Name = "Transport" }
        );

        // Configure Car-Category relationship
        modelBuilder.Entity<Car>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Cars)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Rental-Car relationship
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Car)
            .WithMany(c => c.Rentals)
            .HasForeignKey(r => r.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure indexes for performance
        modelBuilder.Entity<Car>()
            .HasIndex(c => c.RegNr)
            .IsUnique();

        modelBuilder.Entity<Rental>()
            .HasIndex(r => r.BookingNumber)
            .IsUnique();

        modelBuilder.Entity<Rental>()
            .HasIndex(r => new { r.CarId, r.StartDate, r.EndDate });
    }
}
