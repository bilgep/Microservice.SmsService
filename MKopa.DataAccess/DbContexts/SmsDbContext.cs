using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MKopa.Core.Entities.Enums;
using MKopa.Core.Entities.Providers;
using MKopa.Core.Entities.Sms;


namespace MKopa.DataAccess.DbContexts
{
    public class SmsDbContext : DbContext
    {

        public DbSet<BaseSmsMessage> SmsMessages { get; set; }
        public DbSet<BaseSmsProvider> SmsServiceProviders { get; set; }

        public SmsDbContext(DbContextOptions<SmsDbContext> options)
    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseSmsProvider>()
                 .HasData(
                new BaseSmsProvider
                { Id = Guid.NewGuid().ToString(), CountryCode = CountryCode.Turkiye, Name = "ProviderA", IsPrimary = true },
                new BaseSmsProvider
                { Id = Guid.NewGuid().ToString(), CountryCode = CountryCode.Turkiye, Name = "ProviderB", IsPrimary = false });

            modelBuilder.Entity<BaseSmsMessage>().HasData(
                new BaseSmsMessage().Initialize());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite(_configuration.GetConnectionString("SmsDbConnectionString"));
        }
    }
}
