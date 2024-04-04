using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using shared.Data.Entities;

namespace shared.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Website> Websites { get; set; }
        public DbSet<ContactUsMessage> ContactUsMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.WebsiteId)
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Website)
                .WithMany(w => w.Users)
                .HasForeignKey(u => u.WebsiteId)
                .IsRequired();

            modelBuilder.Entity<ContactUsMessage>()
                .Property(u => u.WebsiteId)
                .IsRequired();

            modelBuilder.Entity<ContactUsMessage>()
                .HasOne(u => u.Website)
                .WithMany(w => w.ContactUsMessages)
                .HasForeignKey(u => u.WebsiteId)
                .IsRequired();
        }
    }
}
