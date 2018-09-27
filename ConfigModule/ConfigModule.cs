using Autofac;
using DBLayer.Repositories;
using System.Data.Entity;
using DBLayer.BisLayer;
using DBLayer;

namespace ConfigModule
{
    public class ConfigModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(EntityDBModel)).As(typeof(DbContext)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(BisUserService)).As(typeof(IEntityService<BisUser>)).InstancePerRequest();
            builder.RegisterType(typeof(BisAccountService)).As(typeof(IEntityService<BisAccount>)).InstancePerRequest();
            builder.RegisterType(typeof(LogAuthenticationService))
                .As(typeof(IEntityService<BisLogAuthentication>))
                .InstancePerRequest();
            builder.RegisterType(typeof(LogConnectService))
                .As(typeof(IEntityService<BisLogConnect>))
                .InstancePerRequest();
            builder.RegisterType(typeof(LogMessagingService))
                .As(typeof(IEntityService<BisLogConnect>))
                .InstancePerRequest();
            base.Load(builder);
        }
    }
}
