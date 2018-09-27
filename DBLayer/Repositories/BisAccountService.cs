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
    public class BisAccountService : IEntityService<BisAccount>
    {
        IGenericRepository<Account> userRep = new AccountRepository(new EntityDBModel());
        IMapper mapper;

        public BisAccountService()
        {
            var config = new MapperConfiguration(c => c.CreateMap<Account, BisAccount>());
            mapper = config.CreateMapper();
            var config1 = new MapperConfiguration(c => c.CreateMap<BisAccount, Account>());
            mapper = config1.CreateMapper();
        }

        public BisAccount AddOrUpdate(BisAccount obj)
        {
            Account account = mapper.Map<Account>(obj);
            userRep.AddOrUpdate(account);
            return mapper.Map<BisAccount>(account);
        }

        public BisAccount Delete(BisAccount obj)
        {
            Account account = userRep.Get(obj.Account_Id);
            userRep.Delete(account);
            return mapper.Map<BisAccount>(account);
        }

        public IQueryable<BisAccount> FindBy(Expression<Func<BisAccount, bool>> predicate)
        {
            return GetAll().ToList().Select(e => mapper.Map<BisAccount>(e)).AsQueryable().Where(predicate);
        }

        public BisAccount Get(int id)
        {
            return mapper.Map<BisAccount>(userRep.Get(id));
        }

        public IQueryable<BisAccount> GetAll()
        {
            return userRep.GetAll().ToList().Select(e => mapper.Map<BisAccount>(e)).AsQueryable();
        }

        public BisAccount GetByName(string name)
        {
            return mapper.Map<BisAccount>(userRep.GetByName(name));
        }
    }
}
