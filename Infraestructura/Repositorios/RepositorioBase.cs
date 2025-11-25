using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MarketingLaPazAPI.Core.Interfaces;
using MarketingLaPazAPI.Infraestructura.Data;

namespace MarketingLaPazAPI.Infraestructura.Repositorios
{
    public class RepositorioBase<T> : IRepositorio<T> where T : class
    {
        protected readonly MarketingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositorioBase(MarketingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> ObtenerPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> ObtenerAsync(Expression<Func<T, bool>> predicado)
        {
            return await _dbSet.Where(predicado).ToListAsync();
        }

        public virtual async Task<T> AgregarAsync(T entidad)
        {
            _dbSet.Add(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }

        public virtual async Task ActualizarAsync(T entidad)
        {
            _dbSet.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public virtual async Task EliminarAsync(T entidad)
        {
            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExisteAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}