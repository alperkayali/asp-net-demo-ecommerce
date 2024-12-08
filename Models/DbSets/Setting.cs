using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class Setting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettingID { get; set; }
        public string? telephone { get; set; }

        [EmailAddress]
        public string? email { get; set; }
        public string? adress { get; set; }
        public int mainpageCount { get; set; }
        public int subpageCount { get; set; }
    }
}
