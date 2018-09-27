using System.Data.Entity;
using Repository;

namespace DBLayer.Repositories
{
    public class LogMessagingRepository : GenericRepository<Log_messaging>
    {
        public LogMessagingRepository(DbContext context) : base(context)
        {
        }
    }
}
