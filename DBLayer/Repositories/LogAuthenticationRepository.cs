using System.Data.Entity;
using Repository;

namespace DBLayer.Repositories
{
    public class LogAuthenticationRepository : GenericRepository<Log_Authentication>
    {
        public LogAuthenticationRepository(DbContext context) : base(context)
        {
        }
    }
}
