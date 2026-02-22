using Microsoft.EntityFrameworkCore;
using Raccoon.Extensions.OpenTelemetry.DemoApi.Models;

namespace Raccoon.Extensions.OpenTelemetry.DemoApi.Data;

public class DemoDbContext(DbContextOptions<DemoDbContext> options) : DbContext(options)
{
    public DbSet<WatchedMovie> WatchedMovie => Set<WatchedMovie>();
}