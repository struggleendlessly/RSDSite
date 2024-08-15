using shared.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace shared.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Website> Websites { get; set; }
        public DbSet<ContactUsMessage> ContactUsMessages { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SubscriptionModule> SubscriptionModules { get; set; }
        public DbSet<SubscriptionStripeInfo> SubscriptionStripeInfos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

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
