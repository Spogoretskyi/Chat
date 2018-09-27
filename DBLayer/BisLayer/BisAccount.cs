using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBLayer.BisLayer
{
    public class BisAccount
    {
        [Key] public int Account_Id { get; set; }
        public int? User_Id { get; set; }
        [StringLength(80)] public string Password { get; set; }
        public int? registration { get; set; }
        public virtual User User { get; set; }
    }
}
