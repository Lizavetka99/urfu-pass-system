using Microsoft.EntityFrameworkCore;

namespace UrfuPassSystem.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Moderator> Moderators { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ImageCheck> ImageChecks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>()
            .HasOne(e => e.Student)
            .WithOne()
            .HasForeignKey<Image>(e => e.StudentId)
            .IsRequired(false);
        modelBuilder.Entity<ImageCheck>()
            .HasOne(e => e.Image)
            .WithOne(e => e.Check)
            .HasForeignKey<ImageCheck>(e => e.ImageId)
            .IsRequired();
    }
}
