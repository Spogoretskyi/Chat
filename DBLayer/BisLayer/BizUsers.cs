using System.ComponentModel.DataAnnotations;

namespace DBLayer.BizLayer
{
    public class BisUsers
    {
        [Key]
        public int Name_id { get; set; }

        [Required]
        [StringLength(20)]
        public string name { get; set; }

        [Required]
        [StringLength(80)]
        public string password { get; set; }

        [Required]
        [StringLength(40)]
        public string email { get; set; }

        [Required]
        [StringLength(12)]
        public string phone { get; set; }

        public int registration { get; set; }
    }
}
