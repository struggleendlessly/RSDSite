using shared.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace shared.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Website> Websites { get; set; }
        public DbSet<ContactUsMessage> ContactUsMessages { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionModule> SubscriptionModules { get; set; }
        public DbSet<SubscriptionStripeInfo> SubscriptionStripeInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Website>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Websites);

            modelBuilder.Entity<ContactUsMessage>()
                .HasOne(e => e.Website)
                .WithMany(e => e.ContactUsMessages)
                .HasForeignKey(e => e.WebsiteId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
