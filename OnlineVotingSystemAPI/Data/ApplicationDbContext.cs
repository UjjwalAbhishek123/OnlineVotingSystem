using Microsoft.EntityFrameworkCore;
using OnlineVotingSystemAPI.Helpers;
using OnlineVotingSystemAPI.Models;

namespace OnlineVotingSystemAPI.Data
{
    //Entity Framework DbContext that interacts with the database
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //Tables for Database
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AdminRole> AdminRoles { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<VotingEvent> VotingEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seeding Roles here
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            // Seeding an initial Admin in Admin table
            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 1, FirstName = "Admin", LastName = "User", Email = "admin@example.com", 
                    Password = PasswordHelper.HashPassword("Admin123!") }
            );

            // Seeding an initial User (same as Admin)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    Password = PasswordHelper.HashPassword("Admin123!")
                }
            );

            //defining relationship for AdminRole
            modelBuilder.Entity<AdminRole>()
                .HasKey(ar => new { ar.AdminId, ar.RoleId }); // Composite key

            modelBuilder.Entity<AdminRole>()
                .HasOne(ar => ar.Admin)
                .WithMany(a => a.Roles)
                .HasForeignKey(ar => ar.AdminId);

            modelBuilder.Entity<AdminRole>()
                .HasOne(ar => ar.Role)
                .WithMany(r => r.AdminRoles)
                .HasForeignKey(ar => ar.RoleId);

            // Defining relationship for Candidate and VotingEvent
            modelBuilder.Entity<Candidate>()
                .HasOne(c => c.VotingEvent) // Corrected this line
                .WithMany(v => v.Candidates)
                .HasForeignKey(c => c.VotingEventId);

        }
    }
}
