using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Spis_Leków.Models;
using System.Globalization;

public class FormularzModel : PageModel
{
    private readonly ApplicationDbContext _dbContext;

    public FormularzModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public string Tresc { get; set; }

    [BindProperty]
    public bool CzyPrzyjalesDawke { get; set; } 

    public bool FormularzWypelniony { get; set; } 

    public List<leki> ListaLekow { get; set; }
    public List<Ilosc_lekow> LiczbaLekow { get; set; }

    public IActionResult OnGet()
    {
        
        string imie = User.FindFirst("Imie")?.Value;
        string nazwisko = User.FindFirst("Nazwisko")?.Value;

       
        int? userId = _dbContext.Uzytkownicy
            .Where(u => u.Imie == imie && u.Nazwisko == nazwisko)
            .Select(u => u.Id)
            .FirstOrDefault();

        if (userId.HasValue)
        {
            var dzisiejszyFormularz = _dbContext.Formularze
                .FirstOrDefault(f => f.Pacjent_id == userId.Value &&
                                     (f.Data == null || f.Data.Date == DateTime.Today));

            if (dzisiejszyFormularz != null)
            {
                FormularzWypelniony = true; 
                CzyPrzyjalesDawke = dzisiejszyFormularz.CzyPrzyjalesDawke; 
               
                Tresc = dzisiejszyFormularz.Tresc;
            }
            else
            {
                FormularzWypelniony = false; 
            }
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        string imie = User.FindFirst("Imie")?.Value;
        string nazwisko = User.FindFirst("Nazwisko")?.Value;

        int? userId = _dbContext.Uzytkownicy
            .Where(u => u.Imie == imie && u.Nazwisko == nazwisko)
            .Select(u => u.Id)
            .FirstOrDefault();

        if (userId.HasValue)
        {
            var dzisiejszyFormularz = _dbContext.Formularze
                .FirstOrDefault(f => f.Pacjent_id == userId.Value &&
                                     (f.Data.Date == DateTime.Today));

            if (dzisiejszyFormularz == null)
            {
                var nowyFormularz = new Formularze
                {
                    Pacjent_id = userId.Value,
                    Dzien_tygodnia = DateTime.Today.ToString("dddd", new CultureInfo("pl-PL")),
                    Data = DateTime.Today,
                    Tresc = Tresc,
                    CzyPrzyjalesDawke = !string.IsNullOrEmpty(Request.Form["CzyPrzyjalesDawke"])
                };

                var lekiUzytkownika = _dbContext.Ilosc_lekow
                   .Where(i => i.Uzytkownik_id == userId.Value)
                   .ToList();

                var dzienTygodnia = string.Empty;
                DayOfWeek dayOfWeek = DateTime.Today.DayOfWeek;

                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        dzienTygodnia = "Poniedzialek";
                        break;
                    case DayOfWeek.Tuesday:
                        dzienTygodnia = "Wtorek";
                        break;
                    case DayOfWeek.Wednesday:
                        dzienTygodnia = "Sroda";
                        break;
                    case DayOfWeek.Thursday:
                        dzienTygodnia = "Czwartek";
                        break;
                    case DayOfWeek.Friday:
                        dzienTygodnia = "Piatek";
                        break;
                    case DayOfWeek.Saturday:
                        dzienTygodnia = "Sobota";
                        break;
                    case DayOfWeek.Sunday:
                        dzienTygodnia = "Niedziela";
                        break;
                }

                foreach (var lek in lekiUzytkownika)
                {
                    if (nowyFormularz.CzyPrzyjalesDawke==true)
                    { 
                     var lekZTabeli = _dbContext.leki.FirstOrDefault(l => l.Id == lek.Lek_id && l.DzienTygodnia == dzienTygodnia);
                    if (lekZTabeli != null)
                    {
                        lekZTabeli.Ilosc -= lek.Ilosc;
                    }

                    }
                   
                }

                _dbContext.Formularze.Add(nowyFormularz);
                _dbContext.SaveChanges();

                return RedirectToPage();
            }
        }

        return Page();
    }
}
