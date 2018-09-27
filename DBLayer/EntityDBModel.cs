namespace DBLayer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EntityDBModel : DbContext
    {
        public EntityDBModel() : base("name=EntityDBModel")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Log_Authentication> Log_Authentication { get; set; }
        public virtual DbSet<Log_Connect> Log_Connect { get; set; }
        public virtual DbSet<Log_messaging> Log_messaging { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Log_messaging)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Sender);
        }
    }
}
