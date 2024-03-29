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

        public DbSet<Uzytkownicy> Uzytkownicy { get; set; }
        public DbSet<Formularze> Formularze { get; set; }
        public DbSet<leki> leki { get; set; }
        public DbSet<Ilosc_lekow> Ilosc_lekow { get; set; }
    }
}