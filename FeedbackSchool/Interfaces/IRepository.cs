using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackSchool.Interfaces
{
    public interface IRepository<T>
        where T : class
    {

        IEnumerable<T> GetAllList();
        Task Add(T item);
        Task Delete(int id);
        Task DeleteAll();
    }
}