
using demoAsp2.Data;
using demoAsp2.Interfaces;
using System.Linq.Expressions;

namespace demoAsp2.Responsitory
{
    public abstract class GenetricRepository<T> : IRepository<T> where T : class
    {


        protected DBAspDemo2Context _context;

        public GenetricRepository(DBAspDemo2Context context)
        {
            _context = context;

        }
        public virtual T Add(T entity)
        {
            return _context.Add(entity).Entity;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsQueryable().Where(predicate).ToList();
        }

        public virtual IEnumerable<T> All()
        {
            return _context.Set<T>().ToList();
        }

        public virtual T Get(int id)
        {
            return _context.Find<T>(id);
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();

        }

        public virtual T Update(T entity)
        {
            return _context.Update<T>(entity).Entity;
        }
    }
}
