namespace DBLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Accounts = new HashSet<Account>();
            Log_Authentication = new HashSet<Log_Authentication>();
            Log_Connect = new HashSet<Log_Connect>();
            Log_messaging = new HashSet<Log_messaging>();
        }

        [Key] public int USER_id { get; set; }
        [StringLength(20)] public string Name { get; set; }
        [StringLength(12)] public string Phone { get; set; }
        [StringLength(256)] public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Log_Authentication> Log_Authentication { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Log_Connect> Log_Connect { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Log_messaging> Log_messaging { get; set; }
    }
}
