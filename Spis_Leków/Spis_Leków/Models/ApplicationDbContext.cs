using Microsoft.EntityFrameworkCore;
using Spis_Leków.Models;

namespace Spis_Leków.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    IConfigurationRoot configuration = new ConfigurationBuilder()
            //       .SetBasePath(Directory.GetCurrentDirectory())
            //       .AddJsonFile("appsettings.json")
            //       .Build();
            //    var connectionString = configuration.GetConnectionString("DefaultConnection");
            //    optionsBuilder.UseSqlServer(connectionString);
            //}
        }

        public DbSet<Uzytkownicy> Uzytkownicy { get; set; }
        public DbSet<Formularze> Formularze { get; set; }
        public DbSet<leki> leki { get; set; }
        public DbSet<Ilosc_lekow> Ilosc_lekow { get; set; }
    }
}