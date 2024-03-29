using System.ComponentModel.DataAnnotations;

namespace Spis_Leków.Models
{
    public class Formularze
    {
        [Key]
        public int Id { get; set; }
        public int Pacjent_id { get; set; }
        public string Dzien_tygodnia { get; set; }
        public DateTime Data { get; set; }
        public string Tresc { get; set; }
        public bool CzyPrzyjalesDawke { get; set; }

    }
}
