using Microsoft.EntityFrameworkCore;
using WeatherService.Domain.Entities;

namespace WeatherService.Infrastructure.Persistence;

public sealed class WeatherDbContext(DbContextOptions<WeatherDbContext> options) : DbContext(options)
{
    public DbSet<WeatherRecord> WeatherRecords =>
        Set<WeatherRecord>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherRecord>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.RawResponse)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();
        });
    }
}