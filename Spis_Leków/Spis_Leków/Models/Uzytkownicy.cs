using System.ComponentModel.DataAnnotations;

namespace Spis_Leków.Models
{
    public class Uzytkownicy
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane.")]
        public string? Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string? Nazwisko { get; set; }

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        public string? Haslo { get; set; }

        [Required]
        public string? Ranga { get; set; }
    }
}
