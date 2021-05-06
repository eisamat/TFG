using Microsoft.EntityFrameworkCore;
using Server.Database.Models;

namespace Server.Database
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasIndex(u => u.Nhc)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasIndex(u => u.Token)
                .IsUnique();
            
            modelBuilder.Entity<Video>()
                .HasIndex(v => v.YoutubeId)
                .IsUnique();

            modelBuilder.Entity<Video>()
                .HasIndex(v => v.Name);

            modelBuilder.Entity<Message>()
                .HasIndex(m => m.CreatedAt); 
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}