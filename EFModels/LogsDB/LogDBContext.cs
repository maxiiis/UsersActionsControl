using Microsoft.EntityFrameworkCore;

namespace EFModels.LogsDB
{
    public class LogDBContext : DbContext
    {
        //public LogDBContext(DbContextOptions<LogDBContext> ConnectionString) : base(ConnectionString)
        //{

        //}

        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=LogDB;Username=postgres;Password=password");
        }
    }
}
