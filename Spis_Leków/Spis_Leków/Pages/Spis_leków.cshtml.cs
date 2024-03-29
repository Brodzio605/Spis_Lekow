using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spis_Leków.Models;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Spis_Leków.Pages
{
    public class SpisLekowModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public SpisLekowModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<leki> ListaLekow { get; set; }
        public List<Ilosc_lekow> LiczbaLekow { get; set; }
        public List<Uzytkownicy> Pacjenci { get; set; }
        public List<Formularze> ListaFormularzy { get; set; }
        public List<leki> ListaLekow2 { get; set; }

        public int? WybranyUzytkownikId { get; set; }

        public void OnGet()
        {

            string imie = User.FindFirst("Imie")?.Value;
            string nazwisko = User.FindFirst("Nazwisko")?.Value;


            int? userId = _dbContext.Uzytkownicy
                .Where(u => u.Imie == imie && u.Nazwisko == nazwisko)
                .Select(u => u.Id)
                .FirstOrDefault();

            Pacjenci = _dbContext.Uzytkownicy
                .Where(u => u.Ranga == "pacjent")
                .ToList();

            var dzienTygodnia = DateTime.Today.DayOfWeek;
            var dzienTygodniaString = string.Empty;

            switch (dzienTygodnia)
            {
                case DayOfWeek.Monday:
                    dzienTygodniaString = "Poniedzialek";
                    break;
                case DayOfWeek.Tuesday:
                    dzienTygodniaString = "Wtorek";
                    break;
                case DayOfWeek.Wednesday:
                    dzienTygodniaString = "Sroda";
                    break;
                case DayOfWeek.Thursday:
                    dzienTygodniaString = "Czwartek";
                    break;
                case DayOfWeek.Friday:
                    dzienTygodniaString = "Piatek";
                    break;
                case DayOfWeek.Saturday:
                    dzienTygodniaString = "Sobota";
                    break;
                case DayOfWeek.Sunday:
                    dzienTygodniaString = "Niedziela";
                    break;
            }

            if (userId.HasValue && User.FindFirst("Ranga")?.Value != "moderator")
            {
                var lekiUzytkownika = _dbContext.Ilosc_lekow
                    .Where(i => i.Uzytkownik_id == userId.Value)
                    .Select(i => i.Lek_id)
                    .ToList();

                ListaLekow = _dbContext.leki
                    .Where(l => lekiUzytkownika.Contains(l.Id) && l.DzienTygodnia == dzienTygodniaString)
                    .ToList();

                LiczbaLekow = _dbContext.Ilosc_lekow
                    .Where(i => lekiUzytkownika.Contains(i.Lek_id))
                    .ToList();


            }
            else if (!string.IsNullOrEmpty(Request.Query["uzytkownikId"]))
            {
                var wybranyUzytkownikId = int.Parse(Request.Query["uzytkownikId"]);
                var wybranyUzytkownik = Pacjenci.FirstOrDefault(u => u.Id == wybranyUzytkownikId);

                if (wybranyUzytkownik != null)
                {
                    var lekiUzytkownika = _dbContext.Ilosc_lekow
                        .Where(i => i.Uzytkownik_id == wybranyUzytkownikId)
                        .Select(i => i.Lek_id)
                        .ToList();

                    ListaLekow = _dbContext.leki
                        .Where(l => lekiUzytkownika.Contains(l.Id) && l.DzienTygodnia == dzienTygodniaString)
                        .ToList();


                    ListaLekow2 = _dbContext.leki
                        .Where(l => lekiUzytkownika.Contains(l.Id))
                        .ToList();

                    LiczbaLekow = _dbContext.Ilosc_lekow
                        .Where(i => lekiUzytkownika.Contains(i.Lek_id))
                        .ToList();

                    ListaFormularzy = _dbContext.Formularze
                   .Where(f => f.Pacjent_id == wybranyUzytkownikId)
                      .ToList();




                }
            }

        }
        public IActionResult OnPost(int wybranyLekId, int iloscLeku)
        {
            var lekToUpdate = _dbContext.leki.FirstOrDefault(l => l.Id == wybranyLekId);

            if (lekToUpdate != null)
            {
                lekToUpdate.Ilosc += iloscLeku;
                _dbContext.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}