using FastFoodBot.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastFoodBot.Data;

public class AppDbContext : DbContext
{
    public DbSet<AppUser>? Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}