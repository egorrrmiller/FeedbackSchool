using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedbackSchool.Data
{
    public interface IRepository<T, U>
        where T : class
        where U : class
    {

        IEnumerable<T> GetAllList();
        Task AddFeedback(T item);
        Task DeleteFeedback(int id);
        Task DeleteAllFeedback();

        IEnumerable<U> GetSchoolClass();
        Task AddSchool(U item);
        Task AddClass(U item);
        Task DeleteSchoolOrClass(U item);
        
    }
}