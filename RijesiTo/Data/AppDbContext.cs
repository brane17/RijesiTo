using Microsoft.EntityFrameworkCore;
using RijesiTo.Models;
using Task = RijesiTo.Models.Task;

namespace RijesiTo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Admin", LastName = "", Email = "admin@example.com", Password = "pass123", Role = UserRole.Admin },
                new User { Id = 2, FirstName = "Ivan", LastName = "Horvat", Email = "ivan@example.com", Password = "pass123", Role = UserRole.Client},
                new User { Id = 3, FirstName = "Ana", LastName = "Kovač", Email = "ana@example.com", Password = "pass456", Role = UserRole.Worker }

            );

            // Tasks
            modelBuilder.Entity<Task>().HasData(
                new Task
                {
                    Id = 1,
                    Title = "Popravak slavine",
                    Description = "Slavina curi i treba ju zamijeniti",
                    DateTime = DateTime.Now.AddDays(2),
                    Location = "Zagreb",
                    DepositAmount = 40,
                    Status = Models.TaskStatus.Completed,
                    UserId = 1 // Ivan (Client)
                },
                new Task
                {
                    Id = 2,
                    Title = "Montaža police",
                    Description = "Potrebno montirati zidnu policu",
                    DateTime = DateTime.Now.AddDays(1),
                    Location = "Split",
                    DepositAmount = 20,
                    Status= Models.TaskStatus.NotStarted,
                    UserId = 1 // Ivan (Client)

                }
            );

            // Offers
            modelBuilder.Entity<Offer>().HasData(
                new Offer
                {
                    Id = 1,
                    TaskId = 1,
                    UserId = 2, // Ana
                    OfferDate = DateTime.Now,
                    OfferStatus = OfferStatus.Pending
                }
            );

            // Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    TaskId = 1,
                    UserId = 3, // Marko
                    Rating = 5,
                    Comment = "Vrlo zadovoljan uslugom!",
                    Date = DateTime.Now
                }
            );
        }
    }
}
