using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBLayer.BisLayer
{
    public class BisLogConnect
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Log_c_Id { get; set; }

        public int? User_id { get; set; }
        public int? Action { get; set; }
        public DateTime? Time { get; set; }
        public virtual User User { get; set; }
    }
}
