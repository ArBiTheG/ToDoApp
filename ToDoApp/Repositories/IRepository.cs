using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Repositories
{
    public interface IRepository<T> where T : IEntity, new()
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Func<T, bool> predicate);
        Task<T?> GetByID(int id);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
