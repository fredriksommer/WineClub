using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WineClub.DataAccess.Model;

namespace WineClub.DataAccess
{
    public class WineDbContext : DbContext
    {

        public WineDbContext(DbContextOptions<WineDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new();
            sqlConnectionStringBuilder.DataSource = "Donau.hiof.no";
            sqlConnectionStringBuilder.InitialCatalog = "fredris";
            sqlConnectionStringBuilder.UserID = "fredris";
            sqlConnectionStringBuilder.Password = "...";

            _ = optionsBuilder.UseSqlServer(sqlConnectionStringBuilder.ToString());
        }

        public DbSet<Wine> Wines { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Grape> Grapes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Winery> Wineries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WineEvent> WineEvents { get; set; }
    }

}
