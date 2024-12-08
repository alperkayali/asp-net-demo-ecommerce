namespace AspNetCore_WebAPP_MVC_PROJE.Models.API
{
    public class Pharmacy
    {        
        public DateTime Tarih { get; set; }
        public string? LokasyonY { get; set; }
        public string? LokasyonX { get; set; }
        public string? Adi { get; set; }
        public string? Telefon { get; set; }
        public string? Adres { get; set; }

        //WEB API LINK: https://openapi.izmir.bel.tr/api/ibb/nobetcieczaneler
        //NuGet Package: Newtonsoft.Json
    }
}
