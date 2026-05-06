using AquaCulture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AquaCulture.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

            // Only seed if the database is empty
            if (await context.FishFarms.AnyAsync())
            {
                logger.LogInformation("Database already has data — skipping seed.");
                return;
            }

            logger.LogInformation("Seeding database with test data...");

            // ── Fish Farms ──────────────────────────────────────────────

            var farm1 = new FishFarm
            {
                Id = Guid.NewGuid(),
                Name = "Nordic Salmon Farm",
                GpsLocation = new GeoLocation(63.4305m, 10.3951m),
                NoOfCages = 12,
                HasBarge = true,
                PictureUrl = "https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=800"
            };

            var farm2 = new FishFarm
            {
                Id = Guid.NewGuid(),
                Name = "Fjord Trout Aquaculture",
                GpsLocation = new GeoLocation(60.3913m, 5.3221m),
                NoOfCages = 8,
                HasBarge = false,
                PictureUrl = "https://images.unsplash.com/photo-1534043464124-3f167b5e7b89?w=800"
            };

            var farm3 = new FishFarm
            {
                Id = Guid.NewGuid(),
                Name = "Coastal Shrimp Station",
                GpsLocation = new GeoLocation(58.9700m, 5.7331m),
                NoOfCages = 6,
                HasBarge = true,
                PictureUrl = "https://images.unsplash.com/photo-1498654200943-1088dd4438ae?w=800"
            };

            var farm4 = new FishFarm
            {
                Id = Guid.NewGuid(),
                Name = "Arctic Cod Farm",
                GpsLocation = new GeoLocation(69.6496m, 18.9560m),
                NoOfCages = 20,
                HasBarge = true,
                PictureUrl = "https://images.unsplash.com/photo-1519681393784-d120267933ba?w=800"
            };

            context.FishFarms.AddRange(farm1, farm2, farm3, farm4);

            // ── Workers ─────────────────────────────────────────────────

            var workers = new List<Worker>
            {
                // Farm 1 workers
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Erik Hansen",
                    Age = 42,
                    Email = "erik.hansen@nordicsalmon.no",
                    Position = CrewRole.ChiefExecutiveOfficer,
                    CertifiedUntil = new DateOnly(2027, 6, 15),
                    FishFarmId = farm1.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/men/1.jpg"
                },
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Ingrid Larsen",
                    Age = 35,
                    Email = "ingrid.larsen@nordicsalmon.no",
                    Position = CrewRole.Captain,
                    CertifiedUntil = new DateOnly(2027, 3, 20),
                    FishFarmId = farm1.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/women/2.jpg"
                },
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Lars Olsen",
                    Age = 28,
                    Email = "lars.olsen@nordicsalmon.no",
                    Position = CrewRole.Worker,
                    CertifiedUntil = new DateOnly(2026, 12, 1),
                    FishFarmId = farm1.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/men/3.jpg"
                },
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Maja Eriksen",
                    Age = 31,
                    Email = "maja.eriksen@nordicsalmon.no",
                    Position = CrewRole.Worker,
                    CertifiedUntil = new DateOnly(2027, 9, 10),
                    FishFarmId = farm1.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/women/4.jpg"
                },

                // Farm 2 workers
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Olav Berg",
                    Age = 50,
                    Email = "olav.berg@fjordtrout.no",
                    Position = CrewRole.ChiefExecutiveOfficer,
                    CertifiedUntil = new DateOnly(2028, 1, 30),
                    FishFarmId = farm2.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/men/5.jpg"
                },
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Sigrid Haugen",
                    Age = 39,
                    Email = "sigrid.haugen@fjordtrout.no",
                    Position = CrewRole.Captain,
                    CertifiedUntil = new DateOnly(2027, 7, 5),
                    FishFarmId = farm2.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/women/6.jpg"
                },
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Kristian Dahl",
                    Age = 24,
                    Email = "kristian.dahl@fjordtrout.no",
                    Position = CrewRole.Worker,
                    CertifiedUntil = new DateOnly(2026, 11, 15),
                    FishFarmId = farm2.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/men/7.jpg"
                },

                // Farm 3 workers
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Astrid Nilsen",
                    Age = 45,
                    Email = "astrid.nilsen@coastalshrimp.no",
                    Position = CrewRole.ChiefExecutiveOfficer,
                    CertifiedUntil = new DateOnly(2028, 4, 12),
                    FishFarmId = farm3.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/women/8.jpg"
                },
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Magnus Pedersen",
                    Age = 33,
                    Email = "magnus.pedersen@coastalshrimp.no",
                    Position = CrewRole.Worker,
                    CertifiedUntil = new DateOnly(2027, 2, 28),
                    FishFarmId = farm3.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/men/9.jpg"
                },

                // Farm 4 workers
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Torbjørn Svensen",
                    Age = 55,
                    Email = "torbjorn.svensen@arcticcod.no",
                    Position = CrewRole.ChiefExecutiveOfficer,
                    CertifiedUntil = new DateOnly(2028, 8, 20),
                    FishFarmId = farm4.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/men/10.jpg"
                },
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Hilde Johansen",
                    Age = 37,
                    Email = "hilde.johansen@arcticcod.no",
                    Position = CrewRole.Captain,
                    CertifiedUntil = new DateOnly(2027, 5, 18),
                    FishFarmId = farm4.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/women/11.jpg"
                },
                new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = "Anders Moen",
                    Age = 26,
                    Email = "anders.moen@arcticcod.no",
                    Position = CrewRole.Worker,
                    CertifiedUntil = new DateOnly(2026, 10, 30),
                    FishFarmId = farm4.Id,
                    ProfileImageUrl = "https://randomuser.me/api/portraits/men/12.jpg"
                },
            };

            context.Workers.AddRange(workers);

            await context.SaveChangesAsync();
            logger.LogInformation("Database seeded with {FarmCount} farms and {WorkerCount} workers.",
                4, workers.Count);
        }
    }
}
