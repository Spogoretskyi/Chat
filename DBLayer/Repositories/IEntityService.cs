using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace DBLayer.Repositories
{
    public interface IEntityService<T> where T : class, new()
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Get(int id);
        T AddOrUpdate(T obj);
        T Delete(T obj);
    }
}
