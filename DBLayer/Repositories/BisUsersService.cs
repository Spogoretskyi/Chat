using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using AutoMapper;
using DBLayer.BizLayer;
using Repository;

namespace DBLayer.Repositories
{
    public class BisUsersService : IEntityService<BisUsers>
    {
        IGenericRepository<Users> usersRep = new UsersRepository(new DBmodel());
        IMapper mapper;

        public BisUsersService()
        {
            var config = new MapperConfiguration(c => c.CreateMap<Users, BisUsers>());
            mapper = config.CreateMapper();
            var config1 = new MapperConfiguration(c => c.CreateMap<BisUsers, Users>());
            mapper = config1.CreateMapper();
        }

        public BisUsers AddOrUpdate(BisUsers obj)
        {
            Users user = mapper.Map<Users>(obj);
            usersRep.AddOrUpdate(user);
            return mapper.Map<BisUsers>(user);
        }

        public BisUsers Delete(BisUsers obj)
        {
            Users user = usersRep.Get(obj.Name_id);
            usersRep.Delete(user);
            return mapper.Map<BisUsers>(user);
        }

        public IQueryable<BisUsers> FindBy(Expression<Func<BisUsers, bool>> predicate)
        {
            return GetAll()
                .ToList()
                .Select(e => mapper.Map<BisUsers>(e)).AsQueryable().Where(predicate);
        }

        public BisUsers Get(int id)
        {
            return mapper.Map<BisUsers>(usersRep.Get(id));
        }

        public IQueryable<BisUsers> GetAll()
        {
            return usersRep.GetAll()
                .ToList()
                .Select(e => mapper.Map<BisUsers>(e)).AsQueryable();
        }
    }
}
