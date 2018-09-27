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
    public class BisUserService : IEntityService<BisUser>
    {
        IGenericRepository<User> userRep = new UserRepository(new EntityDBModel());
        IMapper mapper;

        public BisUserService()
        {
            var config = new MapperConfiguration(c => c.CreateMap<User, BisUser>());
            mapper = config.CreateMapper();
            var config1 = new MapperConfiguration(c => c.CreateMap<BisUser, User>());
            mapper = config1.CreateMapper();
        }

        public BisUser AddOrUpdate(BisUser obj)
        {
            User user = mapper.Map<User>(obj);
            userRep.AddOrUpdate(user);
            return mapper.Map<BisUser>(user);
        }

        public BisUser Delete(BisUser obj)
        {
            User user = userRep.Get(obj.USER_id);
            userRep.Delete(user);
            return mapper.Map<BisUser>(user);
        }

        public IQueryable<BisUser> FindBy(Expression<Func<BisUser, bool>> predicate)
        {
            return GetAll().ToList().Select(e => mapper.Map<BisUser>(e)).AsQueryable().Where(predicate);
        }

        public BisUser Get(int id)
        {
            return mapper.Map<BisUser>(userRep.Get(id));
        }

        public BisUser GetByName(string name)
        {
            return mapper.Map<BisUser>(userRep.GetByName(name));
        }

        public IQueryable<BisUser> GetAll()
        {
            return userRep.GetAll().ToList().Select(e => mapper.Map<BisUser>(e)).AsQueryable();
        }
    }
}
