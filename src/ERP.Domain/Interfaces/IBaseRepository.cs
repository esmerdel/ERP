using System.Linq.Expressions;

namespace ERP.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
        Task AdicionarAsync(T entidade);
        Task AtualizarAsync(T entidade);
        Task RemoverAsync(T entidade);
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate);
    }
}
