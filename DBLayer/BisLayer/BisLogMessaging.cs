using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBLayer.BisLayer
{
    public class BisLogMessaging
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Message_Id { get; set; }

        public int? Sender { get; set; }
        public string message { get; set; }
        [StringLength(256)] public string file_name { get; set; }
        [StringLength(30)] public string reciever { get; set; }
        public DateTime? Time { get; set; }
        public virtual User User { get; set; }
    }
}
