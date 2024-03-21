namespace StudentProject.Data.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetStudentByIdAsync(int id, bool useNoTracking = false);
        Task<int> CreateStudentAsync(Student student);
        Task<int> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(Student student);
    }
}
