using Microsoft.EntityFrameworkCore;
using UrfuPassSystem.Domain.Entities;

namespace UrfuPassSystem.Domain.Services;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Institute> Institutes { get; private set; }
    public DbSet<Organization> Organizations { get; private set; }
    public DbSet<Employee> Employees { get; private set; }
    public DbSet<Student> Students { get; private set; }
    public DbSet<Image> Images { get; private set; }
    public DbSet<ImageCheck> ImageChecks { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organization>()
            .HasMany(e => e.Institutes)
            .WithMany();
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Organization)
            .WithMany(e => e.Employees)
            .HasForeignKey(e => e.OrganizationId)
            .IsRequired(false);
        modelBuilder.Entity<Student>()
            .HasOne(e => e.Institute)
            .WithMany()
            .HasForeignKey(e => e.InstituteId);
        modelBuilder.Entity<ImageCheck>()
            .HasOne(e => e.Image)
            .WithMany(e => e.Checks)
            .HasForeignKey(e => e.ImageId);
        modelBuilder.Entity<ImageCheck>()
            .HasOne(e => e.Employee)
            .WithMany()
            .HasForeignKey(e => e.EmployeeId);
    }
}
