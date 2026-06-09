using Microsoft.EntityFrameworkCore;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Persistence;

namespace WeatherService.Infrastructure.Repositories;

public sealed class WeatherRepository(WeatherDbContext dbContext) : IWeatherRepository
{
    public async Task AddAsync(
        WeatherRecord record,
        CancellationToken cancellationToken)
    {
        await dbContext.WeatherRecords
            .AddAsync(record, cancellationToken);

        await dbContext.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<WeatherRecord?> GetLatestAsync(
        CancellationToken cancellationToken)
    {
        return await dbContext.WeatherRecords
            .OrderByDescending(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);
    }
}