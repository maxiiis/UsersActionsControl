

using Microsoft.EntityFrameworkCore;

namespace EFModels.MainDB
{
    public class MainDBContext : DbContext
    {
        //public MainDBContext(DbContextOptions<MainDBContext> ConnectionString) : base(ConnectionString)
        //{
            
        //}
        public DbSet<System> Systems { get; set; }
        public DbSet<BP> BPs { get; set; }
        public DbSet<BPCase> BPCases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MainDB;Username=postgres;Password=password");
        }
    }
}
