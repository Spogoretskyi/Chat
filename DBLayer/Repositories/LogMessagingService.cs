using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using AutoMapper;
using Repository;
using DBLayer.BisLayer;

namespace DBLayer.Repositories
{
    public class LogMessagingService : IEntityService<BisLogMessaging>
    {
        IGenericRepository<Log_messaging> userRep = new LogMessagingRepository(new EntityDBModel());
        IMapper mapper;

        public LogMessagingService()
        {
            var config = new MapperConfiguration(c => c.CreateMap<Log_messaging, BisLogMessaging>());
            mapper = config.CreateMapper();
            var config1 = new MapperConfiguration(c => c.CreateMap<BisLogMessaging, Log_messaging>());
            mapper = config1.CreateMapper();
        }

        public BisLogMessaging AddOrUpdate(BisLogMessaging obj)
        {
            Log_messaging log = mapper.Map<Log_messaging>(obj);
            userRep.AddOrUpdate(log);
            return mapper.Map<BisLogMessaging>(log);
        }

        public BisLogMessaging Delete(BisLogMessaging obj)
        {
            Log_messaging log = userRep.Get(Convert.ToInt32(obj.User.USER_id));
            userRep.Delete(log);
            return mapper.Map<BisLogMessaging>(log);
        }

        public IQueryable<BisLogMessaging> FindBy(Expression<Func<BisLogMessaging, bool>> predicate)
        {
            return GetAll().ToList().Select(e => mapper.Map<BisLogMessaging>(e)).AsQueryable().Where(predicate);
        }

        public BisLogMessaging Get(int id)
        {
            return mapper.Map<BisLogMessaging>(userRep.Get(id));
        }

        public BisLogMessaging GetByName(string name)
        {
            return mapper.Map<BisLogMessaging>(userRep.GetByName(name));
        }

        public IQueryable<BisLogMessaging> GetAll()
        {
            return userRep.GetAll().ToList().Select(e => mapper.Map<BisLogMessaging>(e)).AsQueryable();
        }
    }
}
