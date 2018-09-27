namespace DBLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Log_Authentication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Log_a_Id { get; set; }

        public int? User_id { get; set; }
        public int? Action { get; set; }
        public DateTime? Time { get; set; }
        public virtual User User { get; set; }
    }
}
