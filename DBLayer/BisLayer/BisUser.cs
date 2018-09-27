using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBLayer.BisLayer
{
    public class BisUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BisUser()
        {
            Accounts = new HashSet<BisAccount>();
            Log_Authentication = new HashSet<BisLogAuthentication>();
            Log_Connect = new HashSet<BisLogConnect>();
            Log_messaging = new HashSet<BisLogMessaging>();
        }

        [Key] public int USER_id { get; set; }
        [StringLength(20)] public string Name { get; set; }
        [StringLength(12)] public string Phone { get; set; }
        [StringLength(256)] public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BisAccount> Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BisLogAuthentication> Log_Authentication { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BisLogConnect> Log_Connect { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BisLogMessaging> Log_messaging { get; set; }
    }
}
