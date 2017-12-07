using Autofac;
using DBLayer.Repositories;
using System.Data.Entity;
using DBLayer.BizLayer;
using DBLayer;

namespace ConfigModule
{
    public class ConfigModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(DBmodel)).As(typeof(DbContext))
                .InstancePerLifetimeScope();
            builder.RegisterType(typeof(BisUsersService)).As(typeof(IEntityService<BisUsers>))
                .InstancePerRequest();
            base.Load(builder);
        }
    }
}
