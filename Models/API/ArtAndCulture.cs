namespace AspNetCore_WebAPP_MVC_PROJE.Models.API
{
    public class ArtAndCulture
    {
        public string? Tur { get; set; }
        public int Id { get; set; }
        public string? Adi { get; set; }
        public DateTime? EtkinlikBitisTarihi { get; set; }
        public string? KucukAfis { get; set; }
        public string? EtkinlikMerkezi { get; set; }
        public string? KisaAciklama { get; set; }
        public string? BiletSatisLinki { get; set; }
        public bool UcretsizMi { get; set; }
        public string? Resim { get; set; }
        public string? EtkinlikUrl { get; set; }
        public DateTime? EtkinlikBaslamaTarihi { get; set; }

        //WEB API LINK:https://openapi.izmir.bel.tr/api/ibb/kultursanat/etkinlikler
    }
}
