using System.Linq.Expressions;

namespace StudentProject.Data.Repository
{
    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetStudentByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<T> CreateStudentAsync(T dbRecord);
        Task<T> UpdateStudentAsync(T dbRecord);
        Task<T> DeleteStudentAsync(T dbRecord);
    }
}
