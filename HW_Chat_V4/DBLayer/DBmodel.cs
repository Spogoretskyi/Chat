namespace DBLayer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBmodel : DbContext
    {
        public DBmodel()
            : base("name=DBmodel")
        {
        }

        public virtual DbSet<Users> UsersAll { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
