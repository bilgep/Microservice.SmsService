using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MKopa.DataAccess.DbContexts;

namespace MKopa.Core.Extensions
{
    public static class SqlLiteBuilderExtension
    {
        public static WebApplicationBuilder UseSqlLite(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("SmsDbConnectionString");
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            builder.Services.AddDbContext<SmsDbContext>(dbContextOptions => dbContextOptions.UseSqlite(connection));

            return builder;
        }

        public static WebApplication TriggerDatabaseUpdate(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SmsDbContext>();
                db.Database.EnsureCreated();
            }

            return app;
        
        }
    }
}
