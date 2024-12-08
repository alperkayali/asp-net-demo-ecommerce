using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class Content
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentID { get; set; }
        public bool Active { get; set; }
        public string? ContentName { get; set; }
        public string? ContentInfo { get; set; }

    }
}
