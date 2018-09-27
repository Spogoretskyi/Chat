using System.Data.Entity;
using Repository;

namespace DBLayer.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(DbContext context) : base(context)
        {
        }
    }
}
