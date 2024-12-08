namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}