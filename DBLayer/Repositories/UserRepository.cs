using System.Data.Entity;
using Repository;

namespace DBLayer.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}
