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
    public class LogAuthenticationService : IEntityService<BisLogAuthentication>
    {
        IGenericRepository<Log_Authentication> userRep = new LogAuthenticationRepository(new EntityDBModel());
        IMapper mapper;

        public LogAuthenticationService()
        {
            var config = new MapperConfiguration(c => c.CreateMap<Log_Authentication, BisLogAuthentication>());
            mapper = config.CreateMapper();
            var config1 = new MapperConfiguration(c => c.CreateMap<BisLogAuthentication, Log_Authentication>());
            mapper = config1.CreateMapper();
        }

        public BisLogAuthentication AddOrUpdate(BisLogAuthentication obj)
        {
            Log_Authentication log = mapper.Map<Log_Authentication>(obj);
            userRep.AddOrUpdate(log);
            return mapper.Map<BisLogAuthentication>(log);
        }

        public BisLogAuthentication Delete(BisLogAuthentication obj)
        {
            Log_Authentication log = userRep.Get(Convert.ToInt32(obj.User_id));
            userRep.Delete(log);
            return mapper.Map<BisLogAuthentication>(log);
        }

        public IQueryable<BisLogAuthentication> FindBy(Expression<Func<BisLogAuthentication, bool>> predicate)
        {
            return GetAll().ToList().Select(e => mapper.Map<BisLogAuthentication>(e)).AsQueryable().Where(predicate);
        }

        public BisLogAuthentication Get(int id)
        {
            return mapper.Map<BisLogAuthentication>(userRep.Get(id));
        }

        public BisLogAuthentication GetByName(string name)
        {
            return mapper.Map<BisLogAuthentication>(userRep.GetByName(name));
        }

        public IQueryable<BisLogAuthentication> GetAll()
        {
            return userRep.GetAll().ToList().Select(e => mapper.Map<BisLogAuthentication>(e)).AsQueryable();
        }
    }
}
