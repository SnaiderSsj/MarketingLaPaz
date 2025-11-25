using System.Linq.Expressions;

namespace MarketingLaPazAPI.Core.Interfaces
{
    public interface IRepositorio<T> where T : class
    {
        Task<T> ObtenerPorIdAsync(int id);
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task<IEnumerable<T>> ObtenerAsync(Expression<Func<T, bool>> predicado);
        Task<T> AgregarAsync(T entidad);
        Task ActualizarAsync(T entidad);
        Task EliminarAsync(T entidad);
        Task<bool> ExisteAsync(int id);
    }
}