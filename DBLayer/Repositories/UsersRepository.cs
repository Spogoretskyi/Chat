using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Repository;

namespace DBLayer.Repositories
{
    public class UsersRepository : GenericRepository<Users>
    {
        public UsersRepository(DbContext context) : base(context) { } 
    }
}
