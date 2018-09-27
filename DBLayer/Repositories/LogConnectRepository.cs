using System.Data.Entity;
using Repository;

namespace DBLayer.Repositories
{
    public class LogConnectRepository : GenericRepository<Log_Connect>
    {
        public LogConnectRepository(DbContext context) : base(context)
        {
        }
    }
}
