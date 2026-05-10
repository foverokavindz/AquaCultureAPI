using Microsoft.EntityFrameworkCore;
using AquaCulture.Domain.Entities;

namespace AquaCulture.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<FishFarm> FishFarms { get; set; }
        public DbSet<Worker> Workers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FishFarm>(entity =>
            {
                entity.ToTable("fish_farm");

                entity.HasKey(f => f.Id);

                entity.Property(f => f.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.OwnsOne(f => f.GpsLocation, gl =>
                {
                    gl.Property(g => g.Latitude)
                        .HasColumnType("decimal(9,4)");
                    gl.Property(g => g.Longitude)
                        .HasColumnType("decimal(9,4)");

                    // Unique constraint
                    gl.HasIndex(g => new { g.Latitude, g.Longitude })
                        .IsUnique();
                });

                entity.Property(f => f.NoOfCages)
                    .IsRequired();

                entity.Property(f => f.HasBarge)
                    .HasDefaultValue(false);

                entity.Property(f => f.PictureUrl)
                    .HasMaxLength(500);

                entity.Property(f => f.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasQueryFilter(f => !f.IsDeleted);

                entity.HasMany(f => f.Workers)
                    .WithOne(w => w.FishFarm)
                    .HasForeignKey(w => w.FishFarmId)
                    .IsRequired(false)                
                    .OnDelete(DeleteBehavior.SetNull); 
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.ToTable("worker");

                entity.HasKey(w => w.Id);

                entity.Property(w => w.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(w => w.Age)
                    .IsRequired();

                entity.Property(w => w.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                // Unique constraint
                entity.HasIndex(w => w.Email)
                    .IsUnique();

                entity.Property(w => w.Position)
                    .HasConversion<string>();

                entity.Property(w => w.CertifiedUntil)
                    .IsRequired();

                entity.Property(w => w.ProfileImageUrl)
                    .HasMaxLength(500);

                entity.Property(w => w.IsDeleted)
                    .HasDefaultValue(false);

                entity.HasQueryFilter(w => !w.IsDeleted);

            });
        }
    }
}