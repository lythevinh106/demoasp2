
using System.Linq.Expressions;

namespace demoAsp2.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T entity);

        T Update(T entity);

        T Get(int id);


        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void SaveChanges();
    }
}
