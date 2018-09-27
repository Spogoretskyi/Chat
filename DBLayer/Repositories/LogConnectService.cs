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
    public class LogConnectService : IEntityService<BisLogConnect>
    {
        IGenericRepository<Log_Connect> userRep = new LogConnectRepository(new EntityDBModel());
        IMapper mapper;

        public LogConnectService()
        {
            var config = new MapperConfiguration(c => c.CreateMap<Log_Connect, BisLogConnect>());
            mapper = config.CreateMapper();
            var config1 = new MapperConfiguration(c => c.CreateMap<BisLogConnect, Log_Connect>());
            mapper = config1.CreateMapper();
        }

        public BisLogConnect AddOrUpdate(BisLogConnect obj)
        {
            Log_Connect log = mapper.Map<Log_Connect>(obj);
            userRep.AddOrUpdate(log);
            return mapper.Map<BisLogConnect>(log);
        }

        public BisLogConnect Delete(BisLogConnect obj)
        {
            Log_Connect log = userRep.Get(Convert.ToInt32(obj.User_id));
            userRep.Delete(log);
            return mapper.Map<BisLogConnect>(log);
        }

        public IQueryable<BisLogConnect> FindBy(Expression<Func<BisLogConnect, bool>> predicate)
        {
            return GetAll().ToList().Select(e => mapper.Map<BisLogConnect>(e)).AsQueryable().Where(predicate);
        }

        public BisLogConnect Get(int id)
        {
            return mapper.Map<BisLogConnect>(userRep.Get(id));
        }

        public BisLogConnect GetByName(string name)
        {
            return mapper.Map<BisLogConnect>(userRep.GetByName(name));
        }

        public IQueryable<BisLogConnect> GetAll()
        {
            return userRep.GetAll().ToList().Select(e => mapper.Map<BisLogConnect>(e)).AsQueryable();
        }
    }
}
