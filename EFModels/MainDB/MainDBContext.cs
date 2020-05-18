

using Microsoft.EntityFrameworkCore;

namespace EFModels.MainDB
{
    public class MainDBContext : DbContext
    {
        //public MainDBContext(DbContextOptions<MainDBContext> ConnectionString) : base(ConnectionString)
        //{
            
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MainDB;Username=postgres;Password=password");
        }
    }
}
