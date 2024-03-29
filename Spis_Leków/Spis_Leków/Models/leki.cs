namespace Spis_Leków.Models
{
    public class leki
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Producent { get; set; }
        public decimal Cena { get; set; }
        public int Ilosc { get; set; }
        public string DzienTygodnia { set; get; }       
    }
}
